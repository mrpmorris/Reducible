using System;
using System.Collections.Immutable;
using System.Linq;

namespace Morris.Reducible;

public static partial class Reducer
{
	public static IBuilderSource<TState, TRootDelta, TRootDelta, TRootDelta> Given<TState, TRootDelta>() =>
		new GivenBuilder<TState, TRootDelta>();

	public static Func<TState, TRootDelta, ReducerResult<TState>>
		Combine<TState, TRootDelta>(
			params Func<TState, TRootDelta, ReducerResult<TState>>[] reducers)
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