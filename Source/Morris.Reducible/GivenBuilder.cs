using System;

namespace Morris.Reducible;

internal class GivenBuilder<TState, TRootDelta> : IBuilderSource<TState, TRootDelta, TRootDelta, TRootDelta>
{
	public Func<TState, TRootDelta, ReducerResult<TState>>
		Build(Func<TState, TRootDelta, ReducerResult<TState>> next)
	=>
		next;
}
