using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

internal class WhenImmutableListReducedByBuilder<TState, TElement, TElementCollection, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
	: IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
	where TElementCollection: IImmutableList<TElement>
{
	private readonly IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> BuilderSource;
	private readonly Func<TState, TElementCollection> SubStateSelector;
	private readonly Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> ElementReducer;
	private readonly Func<TState, TElementCollection, TState> StateReducer;

	internal WhenImmutableListReducedByBuilder(
		IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
		Func<TState, TElementCollection> subStateSelector,
		Func<TElement, TSourceDeltaProduced, ReducerResult<TElement>> elementReducer,
		Func<TState, TElementCollection, TState> stateReducer)
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
			TElementCollection elements = SubStateSelector(state);

			bool anyChanged = false;
			for (int o = 0; o < elements.Count; o++)
			{
				(bool changed, TElement element) = ElementReducer(elements[o], delta);
				if (changed)
				{
					elements = (TElementCollection)elements.SetItem(o, element);
					anyChanged = true;
				}
			}

			return anyChanged
				? (true, StateReducer(state, elements))
				: (false, state);
		};

		return BuilderSource.Build(process);
	}
}
