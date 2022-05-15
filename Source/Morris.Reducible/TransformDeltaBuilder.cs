﻿using System;

namespace Morris.Reducible;

public static partial class TransformDeltaExtensions
{
	internal class TransformDeltaBuilder<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced, TDeltaProduced>
		: IBuilderSource<TState, TRootDelta, TSourceDeltaProduced, TDeltaProduced>
	{
		private readonly IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> BuilderSource;
		private readonly Func<TSourceDeltaProduced, TDeltaProduced> DeltaMapper;

		public TransformDeltaBuilder(
			IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
			Func<TSourceDeltaProduced, TDeltaProduced> deltaMapper)
		{
			BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
			DeltaMapper = deltaMapper ?? throw new ArgumentNullException(nameof(deltaMapper));
		}

		public Func<TState, TRootDelta, ReducerResult<TState>>
			Build(Func<TState, TDeltaProduced, ReducerResult<TState>> next)
		{
			if (next is null)
				throw new ArgumentNullException(nameof(next));

			Func<TState, TSourceDeltaProduced, ReducerResult<TState>> result =
				(TState state, TSourceDeltaProduced delta) => next(state, DeltaMapper(delta));
			return BuilderSource.Build(result);
		}
	}
}