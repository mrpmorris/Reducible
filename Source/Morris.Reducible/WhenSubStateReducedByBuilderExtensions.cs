using System;

namespace Morris.Reducible;

public static class WhenSubStateReducedByBuilderExtensions
{
	public static IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
		WhenReducedBy<TState, TElement, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, TElement> subStateSelector,
			Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> elementReducer,
			Func<TState, TElement, TState> stateReducer)
	=>
		new WhenSubStateReducedByBuilder<TState, TElement, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			builderSource: builderSource,
			subStateSelector: subStateSelector,
			elementReducer: elementReducer,
			stateReducer: stateReducer);

}
