//using System;

//namespace Morris.Reducible;

//public static partial class Reducer
//{
//	public class ObjectResultMapper<TState, TSubState, TDelta>
//	{
//		private readonly Func<TState, TSubState> SubStateSelector;
//		private readonly Func<TSubState, TDelta, Result<TSubState>> SubStateReducer;

//		internal ObjectResultMapper(
//			Func<TState, TSubState> subStateSelector,
//			Func<TSubState, TDelta, Result<TSubState>> subStateReducer)
//		{
//			SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
//			SubStateReducer = subStateReducer ?? throw new ArgumentNullException(nameof(subStateReducer));
//		}

//		public Func<TState, TDelta, Result<TState>> Then(Func<TState, TSubState, TState> reducer)
//		{
//			if (reducer is null)
//				throw new ArgumentNullException(nameof(reducer));
			
//			return (TState state, TDelta delta) =>
//			{
//				TSubState subState = SubStateSelector(state);
//				(bool changed, subState) = SubStateReducer(subState, delta);

//				return changed
//					? (true, reducer(state, subState))
//					: (false, state);
//			};
//		}
//	}
//}