using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

internal class WhenImmutableArrayReducedByBuilder<TState, TElement, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
	: IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
{
	private readonly IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> BuilderSource;
	private readonly Func<TState, ImmutableArray<TElement>> SubStateSelector;
	private readonly Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> ElementReducer;
	private readonly Func<TState, ImmutableArray<TElement>, TState> StateReducer;

	internal WhenImmutableArrayReducedByBuilder(
		IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
		Func<TState, ImmutableArray<TElement>> subStateSelector,
		Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> elementReducer,
		Func<TState, ImmutableArray<TElement>, TState> stateReducer)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
		StateReducer = stateReducer ?? throw new ArgumentNullException(nameof(stateReducer));
	}

	public Func<TState, TRootDelta, ReducerResult<TState>>
		Build(Func<TState, TSourceDeltaProduced, ReducerResult<TState>> next)
	{
		Func<TState, TSourceDeltaProduced, ReducerResult<TState>> process = (state, delta) =>
		{
			ImmutableArray<TElement> elements = SubStateSelector(state);

			var arrayBuilder = ImmutableArray.CreateBuilder<TElement>();

			bool anyChanged = false;
			for (int o = 0; o < elements.Length; o++)
			{
				(bool changed, TElement element) = ElementReducer(elements[o], delta);
				arrayBuilder.Add(element);
				if (changed)
					anyChanged = true;
			}

			return anyChanged
				? (true, StateReducer(state, arrayBuilder.ToImmutableArray()))
				: (false, state);
		};

		return BuilderSource.Build(process);
	}
}
