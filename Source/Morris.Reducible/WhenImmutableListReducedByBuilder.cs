using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenImmutableListReducedByBuilder<TState, TElement, TDelta>
{
	private readonly IBuilderSource<TState, TDelta> BuilderSource;
	private readonly Func<TState, ImmutableList<TElement>> SubStateSelector;
	private readonly Func<TElement, TDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenImmutableListReducedByBuilder(
		IBuilderSource<TState, TDelta> builderSource,
		Func<TState, ImmutableList<TElement>> subStateSelector,
		Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, ImmutableList<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
		{
			ImmutableList<TElement> elements = SubStateSelector(state);

			bool anyChanged = false;
			for (int o = 0; o < elements.Count; o++)
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

		return BuilderSource.Build(process);
	}
}
