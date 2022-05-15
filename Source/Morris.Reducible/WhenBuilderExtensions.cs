using System;

namespace Morris.Reducible;

public static class WhenBuilderExtensions
{
	public static IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
		When<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(
			this IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TState, TSourceDeltaProduced, bool> condition)
	=>
		new WhenBuilder<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>(builderSource, condition);
}

