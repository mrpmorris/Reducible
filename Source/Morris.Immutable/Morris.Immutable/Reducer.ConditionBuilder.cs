using System;
using System.Collections.Immutable;

namespace Morris.Immutable;

public static partial class Reducer
{
	public class ConditionBuilder<TState, TAction>
	{
		internal ConditionBuilder() { }

		public ResultMapper<TState, TAction> When(Func<TState, TAction, bool> condition) => new ResultMapper<TState, TAction>(condition);

		public ImmutableArrayResultMapper<TState, TElement, TAction> WhenReducedBy<TElement>(
			Func<TState, ImmutableArray<TElement>> subState,
			Func<TElement, TAction, Result<TElement>> condition)
			=>
				new ImmutableArrayResultMapper<TState, TElement, TAction>(subState, condition);
	}
}