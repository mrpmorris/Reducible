using System;

namespace Morris.Reducible;

public interface IBuilderSource<TState, TDelta>
{
	Func<TState, TDelta, ReducerResult<TState>> Build(Func<TState, TDelta, ReducerResult<TState>> next);
}
