using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static class WhenImmutableArrayReducedByBuilderExtensions
{
	public static WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta, TDelta>(
				sourceBuilder,
				subStateSelector,
				delta => delta,
				elementReducer);

	public static WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>
		WhenReducedBy<TState, TElement, TDelta, TOptimizedDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TDelta, TOptimizedDelta> optimizeDelta,
			Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>(
				sourceBuilder,
				subStateSelector,
				optimizeDelta,
				elementReducer);
}
