using System;

namespace Morris.Reducible;

public class GivenBuilder<TState, TRootDelta> : IBuilderSource<TState, TRootDelta, TRootDelta, TRootDelta>
{
	public Func<TState, TRootDelta, ReducerResult<TState>>
		Build(Func<TState, TRootDelta, ReducerResult<TState>> next)
	=>
		next;
}
