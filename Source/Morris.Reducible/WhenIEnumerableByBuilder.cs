using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenIEnumerableReducedByBuilder<TState, TElement, TDelta>
{
	private readonly Builder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, IEnumerable<TElement>> SubStateSelector;
	private readonly Func<TElement, TDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenIEnumerableReducedByBuilder(
		Builder<TState, TDelta> sourceBuilder,
		Func<TState, IEnumerable<TElement>> subStateSelector,
		Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, IEnumerable<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return (TState state, TDelta delta) =>
		{
			IEnumerable<TElement> elements = SubStateSelector(state);

			var list = new List<TElement>();

			bool anyChanged = false;
			foreach(TElement element in elements)
			{
				(bool changed, TElement newElement) = ElementReducer(element, delta);
				list.Add(newElement);
				if (changed)
					anyChanged = true;
			}

			return anyChanged
				? (true, mapper(state, list.ToImmutableArray()))
				: (false, state);
		};
	}
}
