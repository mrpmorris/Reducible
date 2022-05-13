using System;

namespace Morris.Reducible;

public class WhenSubStateReducedByBuilder<TState, TSubState, TDelta>
{
	private readonly Builder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, TSubState> SubStateSelector;
	private readonly Func<TSubState, TDelta, ReducerResult<TSubState>> SubStateReducer;

	internal WhenSubStateReducedByBuilder(
		Builder<TState, TDelta> sourceBuilder,
		Func<TState, TSubState> subStateSelector,
		Func<TSubState, TDelta, ReducerResult<TSubState>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		SubStateReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, TSubState, TState> reducer)
	{
		if (reducer is null)
			throw new ArgumentNullException(nameof(reducer));

		Func<TState, TDelta, ReducerResult<TState>> process =  (state, delta) =>
		{
			TSubState subState = SubStateSelector(state);
			(bool changed, subState) = SubStateReducer(subState, delta);

			return changed
				? (true, reducer(state, subState))
				: (false, state);
		};

		return SourceBuilder.Build(process);
	}
}
