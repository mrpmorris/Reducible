namespace Morris.Reducible;

public record struct ReducerResult<TState>(bool Changed, TState State)
{
	public static implicit operator (bool changed, TState state)(ReducerResult<TState> result)
		=> (result.Changed, result.State);

	public static implicit operator ReducerResult<TState>((bool changed, TState value) value)
		=> new ReducerResult<TState>(value.changed, value.value);
}
