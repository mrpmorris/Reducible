using System;
using System.Collections.Generic;

namespace Morris.Reducible;

public static class WhenIEnumerableReducedByBuilderExtensions
{
	public static WhenIEnumerableReducedByBuilder<TState, TElement, TDelta, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, IEnumerable<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenIEnumerableReducedByBuilder<TState, TElement, TDelta, TDelta>(
				sourceBuilder,
				subStateSelector,
				delta => delta,
				elementReducer);

	public static WhenIEnumerableReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>
		WhenReducedBy<TState, TElement, TDelta, TOptimizedDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, IEnumerable<TElement>> subStateSelector,
			Func<TDelta, TOptimizedDelta> optimizeDelta,
			Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenIEnumerableReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>(
				sourceBuilder,
				subStateSelector,
				optimizeDelta,
				elementReducer);
}
