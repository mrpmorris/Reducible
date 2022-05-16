using System;

namespace Morris.Reducible;

public static class WhenSubStateReducedByBuilderExtensions
{
	public static WhenSubStateReducedByBuilder<TState, TSubState, TDelta>
		WhenReducedBy<TState, TSubState, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, TSubState> subStateSelector,
			Func<TSubState, TDelta, ReducerResult<TSubState>> elementReducer)
		=>
			new WhenSubStateReducedByBuilder<TState, TSubState, TDelta>(sourceBuilder, subStateSelector, elementReducer);
}
