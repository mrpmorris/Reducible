using System.Collections.Immutable;

namespace Morris.Immutable;

public static partial class Reducer
{
	public class ImmutableArrayResultMapper<TState, TElement, TAction>
	{
		private readonly Func<TState, ImmutableArray<TElement>> Selector;
		private readonly Func<TElement, TAction, Result<TElement>> Condition;

		internal ImmutableArrayResultMapper(
			Func<TState, ImmutableArray<TElement>> selector,
			Func<TElement, TAction, Result<TElement>> condition)
		{
			Selector = selector ?? throw new ArgumentNullException(nameof(selector));
			Condition = condition ?? throw new ArgumentNullException(nameof(condition));
		}

		public Func<TState, TAction, Result<TState>> Then(Func<TState, ImmutableArray<TElement>, TState> reducer)
		{
			ArgumentNullException.ThrowIfNull(reducer);
			return (TState state, TAction action) =>
			{
				ImmutableArray<TElement> elements = Selector(state);

				bool anyChanged = false;
				for (int o = 0; o < elements.Length; o++)
				{
					(bool changed, TElement element) = Condition(elements[o], action);
					if (changed)
					{
						elements = elements.SetItem(o, element);
						anyChanged = true;
					}
				}

				return anyChanged
					? (true, reducer(state, elements))
					: (false, state);
			};
		}
	}
}