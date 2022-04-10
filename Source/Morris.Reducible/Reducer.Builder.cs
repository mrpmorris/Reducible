using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Morris.Reducible;

public static partial class Reducer
{
	public static Builder<TState> New<TState>() => new();

	public class Builder<TState>
	{
		private bool Built;
		private ImmutableArray<KeyValuePair<Type, Func<TState, object, Result<TState>>>> TypesAndReducers;

		internal Builder() => TypesAndReducers = ImmutableArray.Create<KeyValuePair<Type, Func<TState, object, Result<TState>>>>();

		public Builder<TState> Add<TDelta>(Func<TState, TDelta, Result<TState>> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));

			EnsureNotBuilt();
			TypesAndReducers = TypesAndReducers.Add(new(typeof(TDelta), (state, delta) => reducer(state, (TDelta)delta)));
			return this;
		}

		public Func<TState, object, Result<TState>> Build()
		{
			EnsureNotBuilt();
			if (TypesAndReducers.Length == 0)
				throw new InvalidOperationException("Must add at least one reducer to build.");

			Built = true;

			var dictionary = TypesAndReducers
				.GroupBy(x => x.Key)
				.ToDictionary(x => x.Key, x => x.Select(x => x.Value));

			return (TState state, object delta) =>
			{
				if (delta is null)
					throw new ArgumentNullException(nameof(delta));

				if (!dictionary.TryGetValue(delta.GetType(), out var reducers))
					return (false, state);

				bool anyChanged = false;
				TState newState = state;
				foreach(var reducer in reducers)
				{
					(bool changed, newState) = reducer(newState, delta);
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
