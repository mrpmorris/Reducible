namespace Morris.Immutable;

public static partial class Reducer
{
	public record struct Result<TState>(bool Changed, TState State)
	{
		public static implicit operator (bool changed, TState state)(Result<TState> result) => (result.Changed, result.State);
		public static implicit operator Result<TState>((bool changed, TState value) value) => new Result<TState>(value.changed, value.value);
	}
}