using System;

namespace Morris.Reducible;

public static class ThenExtensions
{
	public static Func<TState, TRootDelta, ReducerResult<TState>>
		Then<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, TSourceDeltaProduced, ReducerResult<TState>> mapper)
	{
		if (builderSource is null)
			throw new ArgumentNullException(nameof(builderSource));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		var result = (TState state, TSourceDeltaProduced delta) => mapper(state, delta);
		return builderSource.Build(result);
	}

	public static Func<TState, TRootDelta, ReducerResult<TState>>
		Then<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, TSourceDeltaProduced, TState> mapper)
	{
		if (builderSource is null)
			throw new ArgumentNullException(nameof(builderSource));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return builderSource
			.Then((TState state, TSourceDeltaProduced delta) => (true, mapper(state, delta)));
	}

}

