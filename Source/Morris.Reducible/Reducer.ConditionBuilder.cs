using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static partial class Reducer
{
	public class ConditionBuilder<TState, TAction>
	{
		internal ConditionBuilder() { }

		public ResultMapper<TState, TAction> When(Func<TState, TAction, bool> condition) => new ResultMapper<TState, TAction>(condition);

		public Func<TState, TAction, Result<TState>> Then(Func<TState, TAction, Result<TState>> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));

			return reducer;
		}

		public ObjectResultMapper<TState, TSubState, TAction> WhenReducedBy<TSubState>(
			Func<TState, TSubState> subStateSelector,
			Func<TSubState, TAction, Result<TSubState>> reducer)
		=>
			new ObjectResultMapper<TState, TSubState, TAction>(subStateSelector, reducer);

		public ImmutableArrayResultMapper<TState, TElement, TAction> WhenReducedBy<TElement>(
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TElement, TAction, Result<TElement>> reducer)
			=>
				new ImmutableArrayResultMapper<TState, TElement, TAction>(subStateSelector, reducer);
	}
}