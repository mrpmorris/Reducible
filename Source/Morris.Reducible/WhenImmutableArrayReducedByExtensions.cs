using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static class WhenImmutableArrayReducedByExtensions
{
	public static IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
		WhenReducedBy<TState, TElement, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> elementReducer,
			Func<TState, ImmutableArray<TElement>, TState> stateReducer)
	=>
		new WhenImmutableArrayReducedByBuilder<TState, TElement, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			builderSource: builderSource,
			subStateSelector: subStateSelector,
			elementReducer: elementReducer,
			stateReducer: stateReducer);
}

