using System;
using System.Collections.Immutable;

namespace Morris.Reducible;

public class WhenImmutableArrayReducedByBuilder<TState, TElement, TDelta>
{
	private readonly IBuilderSource<TState, TDelta> BuilderSource;
	private readonly Func<TState, ImmutableArray<TElement>> SubStateSelector;
	private readonly Func<TElement, TDelta, ReducerResult<TElement>> ElementReducer;

	internal WhenImmutableArrayReducedByBuilder(
		IBuilderSource<TState, TDelta> builderSource,
		Func<TState, ImmutableArray<TElement>> subStateSelector,
		Func<TElement, TDelta, ReducerResult<TElement>> elementReducer)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		ElementReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
	}

	public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, ImmutableArray<TElement>, TState> mapper)
	{
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
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
				? (true, mapper(state, arrayBuilder.ToImmutableArray()))
				: (false, state);
		};

		return BuilderSource.Build(process);
	}
}
