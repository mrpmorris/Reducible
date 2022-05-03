using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta>
{
	private readonly Builder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, ImmutableArray<TElement>> SubStateSelector;
	private readonly Func<TElement, TDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenImmutableArrayReducedByBuilder(
		Builder<TState, TDelta> sourceBuilder,
		Func<TState, ImmutableArray<TElement>> subStateSelector,
		Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, ImmutableArray<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return (TState state, TDelta delta) =>
		{
			ImmutableArray<TElement> elements = SubStateSelector(state);

			bool anyChanged = false;
			for (int o = 0; o < elements.Length; o++)
			{
				(bool changed, TElement element) = ElementReducer(elements[o], delta);
				if (changed)
				{
					elements = elements.SetItem(o, element);
					anyChanged = true;
				}
			}

			return anyChanged
				? (true, mapper(state, elements))
				: (false, state);
		};
	}
}
