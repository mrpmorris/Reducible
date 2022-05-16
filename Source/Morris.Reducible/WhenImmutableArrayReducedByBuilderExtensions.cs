using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static class WhenImmutableArrayReducedByBuilderExtensions
{
	public static WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta>(sourceBuilder, subStateSelector, elementReducer);

}
