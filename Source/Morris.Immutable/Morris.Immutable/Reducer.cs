using System.Collections.Immutable;

namespace Morris.Immutable;

public static partial class Reducer
{
	public static ConditionBuilder<TState, TAction> Given<TState, TAction>() => new ConditionBuilder<TState, TAction>();

	public static Func<TState, TAction, Result<TState>> Reduce<TState, TAction>(Func<TState, TAction, Result<TState>> reducer) => reducer;

	public static Func<TState, TAction, Result<TState>> Combine<TState, TAction>(params Func<TState, TAction, Result<TState>>[] reducers)
	{
		ArgumentNullException.ThrowIfNull(reducers);
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