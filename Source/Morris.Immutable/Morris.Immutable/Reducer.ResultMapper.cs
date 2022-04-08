namespace Morris.Immutable;

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
			ArgumentNullException.ThrowIfNull(reducer);
			return (TState state, TAction action) => Condition(state, action) ? (true, reducer(state, action)) : (false, state);
		}
	}
}