using System;

namespace Morris.Reducible;

public static class WhenSubStateReducedByBuilderExtensions
{
	public static WhenSubStateReducedByBuilder<TState, TSubState, TDelta, TDelta>
		WhenReducedBy<TState, TSubState, TDelta>(
			this GivenBuilder<TState, TDelta> sourceBuilder,
			Func<TState, TSubState> subStateSelector,
			Func<TSubState, TDelta, ReducerResult<TSubState>> elementReducer)
		=>
			new WhenSubStateReducedByBuilder<TState, TSubState, TDelta, TDelta>(
				sourceBuilder,
				subStateSelector,
				delta => delta,
				elementReducer);

	public static WhenSubStateReducedByBuilder<TState, TSubState, TDelta, TOptimizedDelta>
		WhenReducedBy<TState, TSubState, TDelta, TOptimizedDelta>(
			this GivenBuilder<TState, TDelta> sourceBuilder,
			Func<TState, TSubState> subStateSelector,
			Func<TDelta, TOptimizedDelta> optimizeDelta,
			Func<TSubState, TOptimizedDelta, ReducerResult<TSubState>> elementReducer)
		=>
			new WhenSubStateReducedByBuilder<TState, TSubState, TDelta, TOptimizedDelta>(
				sourceBuilder,
				subStateSelector,
				optimizeDelta,
				elementReducer);
}
