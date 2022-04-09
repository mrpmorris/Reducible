using System;

namespace Morris.Reducible;

public static partial class Reducer
{
	public class ResultMapper<TState, TDelta>
	{
		private readonly Func<TState, TDelta, bool> Condition;

		internal ResultMapper(Func<TState, TDelta, bool> condition)
		{
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		public Func<TState, TDelta, Result<TState>> Then(Func<TState, TDelta, TState> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));
			
			return (TState state, TDelta delta) => Condition(state, delta) ? (true, reducer(state, delta)) : (false, state);
		}
	}
}