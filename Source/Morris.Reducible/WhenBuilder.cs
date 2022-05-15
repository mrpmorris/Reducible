using System;

namespace Morris.Reducible;

internal class WhenBuilder<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
	: IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
{
	private readonly IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> BuilderSource;
	private readonly Func<TState, TSourceDeltaProduced, bool> Condition;

	internal WhenBuilder(
		IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
		Func<TState, TSourceDeltaProduced, bool> condition)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		Condition = condition ?? throw new ArgumentNullException(nameof(condition));
	}

	public Func<TState, TRootDelta, ReducerResult<TState>>
		Build(Func<TState, TSourceDeltaProduced, ReducerResult<TState>> next)
	{
		if (next is null)
			throw new ArgumentNullException(nameof(next));

		Func<TState, TSourceDeltaProduced, ReducerResult<TState>> process = (state, delta) =>
			!Condition(state, delta)
			? (false, state)
			: next(state, delta);

		return BuilderSource.Build(process);
	}
}

