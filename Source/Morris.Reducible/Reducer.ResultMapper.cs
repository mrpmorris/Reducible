using System;

namespace Morris.Reducible;

public static partial class Reducer
{
	public class ResultMapper<TState, TAction>
	{
		private readonly Func<TState, TAction, bool> Condition;

		internal ResultMapper(Func<TState, TAction, bool> condition)
		{
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		public Func<TState, TAction, Result<TState>> Then(Func<TState, TAction, TState> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));
			
			return (TState state, TAction action) => Condition(state, action) ? (true, reducer(state, action)) : (false, state);
		}
	}
}