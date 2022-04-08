using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Morris.Immutable;

public static partial class Reducer
{
	public static Builder<TState> New<TState>() => new Builder<TState>();

	public class Builder<TState>
	{
		private bool Built;
		private ImmutableArray<KeyValuePair<Type, Func<TState, object, Result<TState>>>> TypesAndReducers;

		internal Builder() => TypesAndReducers = ImmutableArray.Create<KeyValuePair<Type, Func<TState, object, Result<TState>>>>();

		public Builder<TState> Add<TAction>(Func<TState, TAction, Result<TState>> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));

			EnsureNotBuilt();
			TypesAndReducers = TypesAndReducers.Add(new(typeof(TAction), (state, action) => reducer(state, (TAction)action)));
			return this;
		}

		public Func<TState, object, Result<TState>> Build()
		{
			EnsureNotBuilt();
			Built = true;

			var dictionary = TypesAndReducers
				.GroupBy(x => x.Key)
				.ToDictionary(x => x.Key, x => x.Select(x => x.Value));

			return (TState state, object action) =>
			{
				if (action is null)
					throw new ArgumentNullException(nameof(action));

				if (!dictionary.TryGetValue(action.GetType(), out var reducers))
					return (false, state);

				bool anyChanged = false;
				TState newState = state;
				foreach(var reducer in reducers)
				{
					(bool changed, newState) = reducer(newState, action);
					anyChanged |= changed;
				}

				return anyChanged
					? (true, newState)
					: (false, state);
			};
		}

		private void EnsureNotBuilt()
		{
			if (Built)
				throw new InvalidOperationException("Reducer has already been built.");
		}

	}
}
