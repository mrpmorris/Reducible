using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenIEnumerableReducedByBuilder<TState, TElement, TDelta>
{
	private readonly IBuilderSource<TState, TDelta> BuilderSource;
	private readonly Func<TState, IEnumerable<TElement>> SubStateSelector;
	private readonly Func<TElement, TDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenIEnumerableReducedByBuilder(
		IBuilderSource<TState, TDelta> builderSource,
		Func<TState, IEnumerable<TElement>> subStateSelector,
		Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, IEnumerable<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
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

		return BuilderSource.Build(process);
	}
}
