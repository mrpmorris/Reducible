using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static  class WhenImmutableListReducedByExtensions
{
	public static IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
		WhenReducedBy<TState, TElement, TElementCollection, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, TElementCollection> subStateSelector,
			Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> elementReducer,
			Func<TState, TElementCollection, TState> stateReducer)
			where TElementCollection : IImmutableList<TElement>

	=>
		new WhenImmutableListReducedByBuilder<TState, TElement, TElementCollection, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			builderSource: builderSource,
			subStateSelector: subStateSelector,
			elementReducer: elementReducer,
			stateReducer: stateReducer);
}
