using System;

namespace Morris.Reducible;

public abstract class Builder<TState, TDelta> 
{
	internal protected abstract Func<TState, TDelta, ReducerResult<TState>> Build(
		Func<TState, TDelta, ReducerResult<TState>> next);
}
