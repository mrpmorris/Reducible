using System;

namespace Morris.Reducible;

public class GivenBuilder<TState, TDelta> : Builder<TState, TDelta>
{
	protected internal override Func<TState, TDelta, ReducerResult<TState>> Build(
		Func<TState, TDelta, ReducerResult<TState>> next)
	=>
		next;
}
