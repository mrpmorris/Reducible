using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenIEnumerableReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>
{
	private readonly GivenBuilder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, IEnumerable<TElement>> SubStateSelector;
	private readonly Func<TDelta, TOptimizedDelta> OptimizeDelta;
	private readonly Func<TElement, TOptimizedDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenIEnumerableReducedByBuilder(
		GivenBuilder<TState, TDelta> sourceBuilder,
		Func<TState, IEnumerable<TElement>> subStateSelector,
		Func<TDelta, TOptimizedDelta> optimizeDelta,
		Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		OptimizeDelta = optimizeDelta ?? throw new ArgumentNullException(nameof(optimizeDelta));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, IEnumerable<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
		{
			TOptimizedDelta optimizedDelta = OptimizeDelta(delta);
			IEnumerable<TElement> elements = SubStateSelector(state);

			var list = new List<TElement>();

			bool anyChanged = false;
			foreach(TElement element in elements)
			{
				(bool changed, TElement newElement) = ElementReducer(element, optimizedDelta);
				list.Add(newElement);
				if (changed)
					anyChanged = true;
			}

			return anyChanged
				? (true, mapper(state, list.ToImmutableArray()))
				: (false, state);
		};

		return SourceBuilder.Build(process);
	}
}
