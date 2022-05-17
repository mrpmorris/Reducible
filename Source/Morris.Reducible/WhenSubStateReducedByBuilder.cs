using System;

namespace Morris.Reducible;

public class WhenSubStateReducedByBuilder<TState, TSubState, TDelta, TOptimizedDelta>
{
	private readonly GivenBuilder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, TSubState> SubStateSelector;
	private readonly Func<TDelta, TOptimizedDelta> OptimizeDelta;
	private readonly Func<TSubState, TOptimizedDelta, ReducerResult<TSubState>> SubStateReducer;

	internal WhenSubStateReducedByBuilder(
		GivenBuilder<TState, TDelta> sourceBuilder,
		Func<TState, TSubState> subStateSelector,
		Func<TDelta, TOptimizedDelta> optimizeDelta,
		Func<TSubState, TOptimizedDelta, ReducerResult<TSubState>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		OptimizeDelta = optimizeDelta ?? throw new ArgumentNullException(nameof(optimizeDelta));
		SubStateReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, TSubState, TState> reducer)
	{
		if (reducer is null)
			throw new ArgumentNullException(nameof(reducer));

		Func<TState, TDelta, ReducerResult<TState>> process =  (state, delta) =>
		{
			TOptimizedDelta optimizedDelta = OptimizeDelta(delta);
			TSubState subState = SubStateSelector(state);

			(bool changed, subState) = SubStateReducer(subState, optimizedDelta);

			return changed
				? (true, reducer(state, subState))
				: (false, state);
		};

		return SourceBuilder.Build(process);
	}
}
