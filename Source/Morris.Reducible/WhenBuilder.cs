using System;

namespace Morris.Reducible;

public class WhenBuilder<TState, TDelta> : IBuilderSource<TState, TDelta>
{
	private readonly IBuilderSource<TState, TDelta> BuilderSource;
	private readonly Func<TState, TDelta, bool> Condition;

	internal WhenBuilder(IBuilderSource<TState, TDelta> builderSource, Func<TState, TDelta, bool> condition)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		Condition = condition ?? throw new ArgumentNullException(nameof(condition));
	}

	public Func<TState, TDelta, ReducerResult<TState>>
		Build(Func<TState, TDelta, ReducerResult<TState>> next)
	{
		if (next is null)
			throw new ArgumentNullException(nameof(next));

		Func<TState, TDelta, ReducerResult<TState>> process = (state, delta) =>
			!Condition(state, delta)
			? (false, state)
			: next(state, delta);

		return BuilderSource.Build(process);
	}
}

