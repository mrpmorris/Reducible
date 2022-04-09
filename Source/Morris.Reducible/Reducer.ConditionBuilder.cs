using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public static partial class Reducer
{
	public class ConditionBuilder<TState, TDelta>
	{
		internal ConditionBuilder() { }

		public ResultMapper<TState, TDelta> When(Func<TState, TDelta, bool> condition) => new ResultMapper<TState, TDelta>(condition);

		public Func<TState, TDelta, Result<TState>> Then(Func<TState, TDelta, Result<TState>> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));

			return reducer;
		}

		public ObjectResultMapper<TState, TSubState, TDelta> WhenReducedBy<TSubState>(
			Func<TState, TSubState> subStateSelector,
			Func<TSubState, TDelta, Result<TSubState>> reducer)
		=>
			new ObjectResultMapper<TState, TSubState, TDelta>(subStateSelector, reducer);

		public ImmutableArrayResultMapper<TState, TElement, TDelta> WhenReducedBy<TElement>(
			Func<TState, ImmutableArray<TElement>> subStateSelector,
			Func<TElement, TDelta, Result<TElement>> reducer)
			=>
				new ImmutableArrayResultMapper<TState, TElement, TDelta>(subStateSelector, reducer);
	}
}