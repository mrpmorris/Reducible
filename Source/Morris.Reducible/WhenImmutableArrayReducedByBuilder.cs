﻿using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenImmutableArrayReducedByBuilder<TState, TElement, TDeltaIn, TOptimizedDelta>
{
	private readonly Builder<TState, TDeltaIn> SourceBuilder;
	private readonly Func<TState, ImmutableArray<TElement>> SubStateSelector;
	private readonly Func<TDeltaIn, TOptimizedDelta> OptimizeDelta;
	private readonly Func<TElement, TOptimizedDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenImmutableArrayReducedByBuilder(
		Builder<TState, TDeltaIn> sourceBuilder,
		Func<TState, ImmutableArray<TElement>> subStateSelector,
		Func<TDeltaIn, TOptimizedDelta> optimizeDelta,
		Func<TElement, TOptimizedDelta, ReducerResult<TElement>> elementReducer)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		OptimizeDelta = optimizeDelta ?? throw new ArgumentNullException(nameof(optimizeDelta));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDeltaIn, ReducerResult<TState>> Then(Func<TState, ImmutableArray<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDeltaIn, ReducerResult<TState>> process = (state, delta) =>
		{
			ImmutableArray<TElement> elements = SubStateSelector(state);

			TOptimizedDelta optimizedDelta = OptimizeDelta(delta);

			var arrayBuilder = ImmutableArray.CreateBuilder<TElement>();

			bool anyChanged = false;
			for (int o = 0; o < elements.Length; o++)
			{
				(bool changed, TElement element) = ElementReducer(elements[o], optimizedDelta);
				arrayBuilder.Add(element);
				if (changed)
					anyChanged = true;
			}

			return anyChanged
				? (true, mapper(state, arrayBuilder.ToImmutableArray()))
				: (false, state);
		};

		return SourceBuilder.Build(process);
	}
}
