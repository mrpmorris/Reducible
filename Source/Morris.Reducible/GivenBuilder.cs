using System;

namespace Morris.Reducible;

public class GivenBuilder<TState, TDelta> : IBuilderSource<TState, TDelta>
{
	public Func<TState, TDelta, ReducerResult<TState>>
		Build(Func<TState, TDelta, ReducerResult<TState>> next)
	=>
		next;
}
