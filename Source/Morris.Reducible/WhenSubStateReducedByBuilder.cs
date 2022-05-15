using System;

namespace Morris.Reducible;

internal class WhenSubStateReducedByBuilder<TState, TSubState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
	: IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced>
{
	private readonly IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> BuilderSource;
	private readonly Func<TState, TSubState> SubStateSelector;
	private readonly Func<TSubState, TSourceDeltaProduced, ReducerResult<TSubState>> SubStateReducer;
	private readonly Func<TState, TSubState, TState> StateReducer;

	internal WhenSubStateReducedByBuilder(
		IBuilderSource<TState, TRootDelta, TSourceDeltaConsumed, TSourceDeltaProduced> builderSource,
		Func<TState, TSubState> subStateSelector,
		Func<TSubState, TSourceDeltaProduced, ReducerResult<TSubState>> elementReducer,
		Func<TState, TSubState, TState> stateReducer)
	{
		BuilderSource = builderSource ?? throw new ArgumentNullException(nameof(builderSource));
		SubStateSelector = subStateSelector ?? throw new ArgumentNullException(nameof(subStateSelector));
		SubStateReducer = elementReducer ?? throw new ArgumentNullException(nameof(elementReducer));
		StateReducer = stateReducer ?? throw new ArgumentNullException(nameof(stateReducer));
	}

	public Func<TState, TRootDelta, ReducerResult<TState>> Build(Func<TState, TSourceDeltaProduced, ReducerResult<TState>> next)
	{
		Func<TState, TSourceDeltaProduced, ReducerResult<TState>> process =  (state, delta) =>
		{
			TSubState subState = SubStateSelector(state);
			(bool changed, subState) = SubStateReducer(subState, delta);

			return changed
				? (true, StateReducer(state, subState))
				: (false, state);
		};

		return BuilderSource.Build(process);
	}
}
