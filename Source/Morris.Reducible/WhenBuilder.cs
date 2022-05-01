﻿using System;

namespace Morris.Reducible;

public static class WhenBuilderExtensions
{
	public static WhenBuilder<TState, TDelta> When<TState, TDelta>(
		this Builder<TState, TDelta> builder,
		Func<TState, TDelta, bool> condition)
		=>
			new WhenBuilder<TState, TDelta>(condition);
}

public class WhenBuilder<TState, TDelta> : Builder<TState, TDelta>
{
	private readonly Func<TState, TDelta, bool> Condition;

	internal WhenBuilder(Func<TState, TDelta, bool> condition)
	{
		Condition = condition ?? throw new ArgumentNullException(nameof(condition));
	}

	protected internal override Func<TState, TDelta, ReducerResult<TState>> Build(
		Func<TState, TDelta, ReducerResult<TState>> next)
	{
		if (next is null)
			throw new ArgumentNullException(nameof(next));
		return (TState state, TDelta delta) =>
			!Condition(state, delta)
			? (false, state)
			: next(state, delta);
	}
}

