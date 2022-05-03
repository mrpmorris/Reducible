using System;
using System.Collections.Generic;
using System.Linq;

namespace Morris.Reducible;

public static partial class Reducer
{
	public static CompositeBuilder<TState> CreateCompositeBuilder<TState>() => new CompositeBuilder<TState>();

	public class CompositeBuilder<TState>
	{
		private bool Built;
		private List<KeyValuePair<Type, Func<TState, object, ReducerResult<TState>>>> TypesAndReducers;

		internal CompositeBuilder()
		{
			TypesAndReducers = new();
		}

		public CompositeBuilder<TState> Add<TDelta>(Func<TState, TDelta, ReducerResult<TState>> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));

			EnsureNotBuilt();
			TypesAndReducers.Add(new(typeof(TDelta), (state, delta) => reducer(state, (TDelta)delta)));
			return this;
		}

		public Func<TState, object, ReducerResult<TState>> Build()
		{
			EnsureNotBuilt();
			if (TypesAndReducers.Count == 0)
				throw new InvalidOperationException("Must add at least one reducer to build");

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
				foreach (var reducer in reducers)
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
