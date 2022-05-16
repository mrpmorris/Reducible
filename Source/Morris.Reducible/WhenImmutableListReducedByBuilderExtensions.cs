using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static class WhenImmutableListReducedByBuilderExtensions
{
	public static WhenImmutableListReducedByBuilder<TState, TElement, TDelta, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableList<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableListReducedByBuilder<TState, TElement, TDelta, TDelta>(
				sourceBuilder,
				subStateSelector,
				delta => delta,
				elementReducer);

	public static WhenImmutableListReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>
		WhenReducedBy<TState, TElement, TDelta, TOptimizedDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableList<TElement>> subStateSelector,
			Func<TDelta, TOptimizedDelta> optimizeDelta,
			Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableListReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>(
				sourceBuilder,
				subStateSelector,
				optimizeDelta,
				elementReducer);

}
