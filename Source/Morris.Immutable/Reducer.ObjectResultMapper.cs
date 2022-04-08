using System;

namespace Morris.Immutable;

public static partial class Reducer
{
	public class ObjectResultMapper<TState, TSubState, TAction>
	{
		private readonly Func<TState, TSubState> SubStateSelector;
		private readonly Func<TSubState, TAction, Result<TSubState>> SubStateReducer;

		internal ObjectResultMapper(
			Func<TState, TSubState> subStateSelector,
			Func<TSubState, TAction, Result<TSubState>> subStateReducer)
		{
			SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
			SubStateReducer = subStateReducer ?? throw new ArgumentNullException(nameof(subStateReducer));
		}

		public Func<TState, TAction, Result<TState>> Then(Func<TState, TSubState, TState> reducer)
		{
			if (reducer is null)
				throw new ArgumentNullException(nameof(reducer));
			
			return (TState state, TAction action) =>
			{
				TSubState subState = SubStateSelector(state);
				(bool changed, subState) = SubStateReducer(subState, action);

				return (changed, state);
			};
		}
	}
}