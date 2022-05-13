using System;

namespace Morris.Reducible;

public static class ThenExtensions
{
	public static Func<TState, TDelta, ReducerResult<TState>> Then<TState, TDelta>(
		this IBuilderSource<TState, TDelta> builderSource,
		Func<TState, TDelta, ReducerResult<TState>> mapper)
	{
		if (builderSource is null)
			throw new ArgumentNullException(nameof(builderSource));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		var result = (TState state, TDelta delta) => mapper(state, delta);
		return builderSource.Build(result);
	}

	public static Func<TState, TDelta, ReducerResult<TState>> Then<TState, TDelta>(
		this IBuilderSource<TState, TDelta> builderSource,
		Func<TState, TDelta, TState> mapper)
	{
		if (builderSource is null)
			throw new ArgumentNullException(nameof(builderSource));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return builderSource
			.Then((TState state, TDelta delta) => (true, mapper(state, delta)));
	}

}

