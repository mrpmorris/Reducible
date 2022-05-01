using System;

namespace Morris.Reducible;

public abstract class Builder<TState, TDelta>
{
	internal protected abstract Func<TState, TDelta, ReducerResult<TState>> Build(
		Func<TState, TDelta, ReducerResult<TState>> next);


	//public ObjectResultMapper<TState, TSubState, TDelta> WhenReducedBy<TSubState>(
	//	Func<TState, TSubState> subStateSelector,
	//	Func<TSubState, TDelta, ReducerResult<TSubState>> reducer)
	//=>
	//	new ObjectResultMapper<TState, TSubState, TDelta>(subStateSelector, reducer);

	//public ImmutableArrayResultMapper<TState, TElement, TDelta> WhenReducedBy<TElement>(
	//	Func<TState, ImmutableArray<TElement>> subStateSelector,
	//	Func<TElement, TDelta, ReducerResult<TElement>> reducer)
	//	=>
	//		new ImmutableArrayResultMapper<TState, TElement, TDelta>(subStateSelector, reducer);

	//public Func<TState, TDelta, ReducerResult<TState>> Then(Func<TState, TDelta, ReducerResult<TState>> reducer)
	//{
	//	if (reducer is null)
	//		throw new ArgumentNullException(nameof(reducer));

	//	return reducer;
	//}
}
