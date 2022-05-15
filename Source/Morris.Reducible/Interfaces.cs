using System;

namespace Morris.Reducible;

public interface IBuilderSource<TState, TRootDelta, in TDeltaConsumed, out TDeltaProduced>
{
	Func<TState, TRootDelta, ReducerResult<TState>> Build(Func<TState, TDeltaProduced, ReducerResult<TState>> next);
}
