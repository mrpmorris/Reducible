using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static class WhenImmutableListReducedByBuilderExtensions
{
	public static WhenImmutableListReducedByBuilder<TState, TElement, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableList<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableListReducedByBuilder<TState, TElement, TDelta>(sourceBuilder, subStateSelector, elementReducer);

}
