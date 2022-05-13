using System;

namespace Morris.Reducible;

public class WhenBuilder<TState, TDelta> : Builder<TState, TDelta>
{
	private readonly Builder<TState, TDelta> Source;
	private readonly Func<TState, TDelta, bool> Condition;

	internal WhenBuilder(Builder<TState, TDelta> source, Func<TState, TDelta, bool> condition)
	{
		Source = source ?? throw new ArgumentNullException(nameof(source));
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

		return Source.Build(process);
	}
}

