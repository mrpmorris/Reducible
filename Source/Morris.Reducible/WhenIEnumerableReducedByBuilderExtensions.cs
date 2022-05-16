using System;
using System.Collections.Generic;

namespace Morris.Reducible;

public static class WhenIEnumerableReducedByBuilderExtensions
{
	public static WhenIEnumerableReducedByBuilder<TState, TElement, TDelta>
		WhenReducedBy<TState, TElement, TDelta>(
			this Builder<TState, TDelta> sourceBuilder,
			Func<TState, IEnumerable<TElement>> subStateSelector,
			Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
		=>
			new WhenIEnumerableReducedByBuilder<TState, TElement, TDelta>(sourceBuilder, subStateSelector, elementReducer);
}
