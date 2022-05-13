using System;

namespace Morris.Reducible;

public class WhenBuilder<TState, TDelta> : Builder<TState, TDelta>
{
	private readonly Builder<TState, TDelta> SourceBuilder;
	private readonly Func<TState, TDelta, bool> Condition;

	internal WhenBuilder(Builder<TState, TDelta> sourceBuilder, Func<TState, TDelta, bool> condition)
	{
		SourceBuilder = sourceBuilder ?? throw new ArgumentNullException(nameof(sourceBuilder));
		Condition = condition ?? throw new ArgumentNullException(nameof(condition));
	}

	protected internal override Func<TState, TDelta, ReducerResult<TState>> Build(
		Func<TState, TDelta, ReducerResult<TState>> next)
	{
		if (next is null)
			throw new ArgumentNullException(nameof(next));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
			!Condition(state, delta)
			? (false, state)
			: next(state, delta);

		return SourceBuilder.Build(process);
	}
}

