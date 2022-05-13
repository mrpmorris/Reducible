using System;
using System.Collections.Immutable;
using System.Linq;

namespace Morris.Reducible;

public static partial class Reducer
{
	public static Builder<TState, TDelta> Given<TState, TDelta>() =>
		new GivenBuilder<TState, TDelta>();

	public static Func<TState, TDelta, ReducerResult<TState>> Combine<TState, TDelta>(
		params Func<TState, TDelta, ReducerResult<TState>>[] reducers)
	{
		if (reducers is null)
			throw new ArgumentNullException(nameof(reducers));
		if (reducers.Length < 2)
			throw new ArgumentException(paramName: nameof(reducers), message: "At least two reducers are required.");
		if (reducers.Any(x => x is null))
			throw new ArgumentException(paramName: nameof(reducers), message: "Reducers cannot be null.");

		var allReducers = reducers.ToImmutableArray();
		return (state, delta) =>
		{
			bool anyChanged = false;
			for (int o = 0; o < allReducers.Length; o++)
			{
				(bool changed, state) = allReducers[o](state, delta);
				anyChanged |= changed;
			}
			return (anyChanged, state);
		};
	}
}