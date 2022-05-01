using System;

namespace Morris.Reducible;

public static class ThenBuilderExtensions
{
	public static Func<TState, TDelta, ReducerResult<TState>> Then<TState, TDelta>(
		this Builder<TState, TDelta> sourceBuilder,
		Func<TState, TDelta, ReducerResult<TState>> mapper)
	{
		if (sourceBuilder is null)
			throw new ArgumentNullException(nameof(sourceBuilder));
		if (mapper is null)
			throw new ArgumentNullException(nameof(mapper));

		return (TState state, TDelta delta) =>
			mapper(state, delta);
	}
}

