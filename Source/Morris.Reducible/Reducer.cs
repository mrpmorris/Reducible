using System;
using System.Collections.Immutable;
using System.Linq;

namespace Morris.Reducible;

public static partial class Reducer
{
	public static ConditionBuilder<TState, TAction> Given<TState, TAction>() => new ConditionBuilder<TState, TAction>();

	public static Builder<TState> CreateBuilder<TState>() => new Builder<TState>();

	public static Func<TState, TAction, Result<TState>> Combine<TState, TAction>(params Func<TState, TAction, Result<TState>>[] reducers)
	{
		if (reducers is null)
			throw new ArgumentNullException(nameof(reducers));
		if (reducers.Length < 2)
			throw new ArgumentException(paramName: nameof(reducers), message: "At least two reducers are required.");
		if (reducers.Any(x => x is null))
			throw new ArgumentException(paramName: nameof(reducers), message: "Reducers cannot be null.");

		var allReducers = reducers.ToImmutableArray();
		return (state, action) =>
		{
			bool anyChanged = false;
			for (int o = 0; o < allReducers.Length; o++)
			{
				(bool changed, state) = allReducers[o](state, action);
				anyChanged |= changed;
			}
			return (anyChanged, state);
		};
	}
}