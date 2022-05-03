//using System;
//using System.Collections.Immutable;

//namespace Morris.Reducible;

//public static partial class Reducer
//{
//	public class ImmutableArrayResultMapper<TState, TElement, TDelta>
//	{
//		private readonly Func<TState, ImmutableArray<TElement>> SubStateSelector;
//		private readonly Func<TElement, TDelta, Result<TElement>> ElementReducer;

//		internal ImmutableArrayResultMapper(
//			Func<TState, ImmutableArray<TElement>> subStateSelector,
//			Func<TElement, TDelta, Result<TElement>> elementReducer)
//		{
//			SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
//			ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
//		}

//		public Func<TState, TDelta, Result<TState>> Then(Func<TState, ImmutableArray<TElement>, TState> reducer)
//		{
//			if (reducer is null)
//				throw new ArgumentNullException(nameof(reducer));
			
//			return (TState state, TDelta delta) =>
//			{
//				ImmutableArray<TElement> elements = SubStateSelector(state);

//				bool anyChanged = false;
//				for (int o = 0; o < elements.Length; o++)
//				{
//					(bool changed, TElement element) = ElementReducer(elements[o], delta);
//					if (changed)
//					{
//						elements = elements.SetItem(o, element);
//						anyChanged = true;
//					}
//				}

//				return anyChanged
//					? (true, reducer(state, elements))
//					: (false, state);
//			};
//		}
//	}
//}