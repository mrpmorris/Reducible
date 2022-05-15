using System;

namespace Morris.Reducible;

public static partial class TransformDeltaExtensions
{
	public static IBuilderSource<TState, TRootDelta, TSourceDeltaProduced, TDeltaProduced>
		TransformDelta<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced, TDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TSourceDeltaProduced, TDeltaProduced> mapper)
	{
		if (builderSource is null)
			throw new ArgumentNullException(nameof(builderSource));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return new TransformDeltaBuilder<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced, TDeltaProduced>(builderSource, mapper);
	}

}
