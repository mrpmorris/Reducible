﻿namespace Morris.Reducible;

public static partial class Reducer
{
	public record struct Result<TState>(bool Changed, TState State)
	{
		public static implicit operator (bool changed, TState state)(Result<TState> result) => (result.Changed, result.State);
		public static implicit operator Result<TState>((bool changed, TState state) tuple) => new(tuple.changed, tuple.state);
	}
}