using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenImmutableListReducedByBuilder<TState, TElement, TDelta, TOptimizedDelta>
{
	private readonly GivenBuilder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, ImmutableList<TElement>> SubStateSelector;
	private readonly Func<TDelta, TOptimizedDelta> OptimizeDelta;
	private readonly Func<TElement, TOptimizedDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenImmutableListReducedByBuilder(
		GivenBuilder<TState, TDelta> sourceBuilder,
		Func<TState, ImmutableList<TElement>> subStateSelector,
		Func<TDelta, TOptimizedDelta> optimizeDelta,
		Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		OptimizeDelta = optimizeDelta ?? throw new ArgumentNullException(nameof(optimizeDelta));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, ImmutableList<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
		{
			TOptimizedDelta optimizedDelta = OptimizeDelta(delta);
			ImmutableList<TElement> elements = SubStateSelector(state);

			bool anyChanged = false;
			for (int o = 0; o < elements.Count; o++)
			{
				(bool changed, TElement element) = ElementReducer(elements[o], optimizedDelta);
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

		return SourceBuilder.Build(process);
	}
}
