using System;

namespace Morris.Reducible;

public static class WhenExtensions
{
	public static WhenBuilder<TState, TDelta> When<TState, TDelta>(
		this Builder<TState, TDelta> source,
		Func<TState, TDelta, bool> condition)
		=>
			new WhenBuilder<TState, TDelta>(source, condition);
}

