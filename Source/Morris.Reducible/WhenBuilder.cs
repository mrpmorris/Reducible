//using System;

//namespace Morris.Reducible;

//public WhenBuilder<TState, TDelta> When<TState, TDelta>(
//	this Builder<TState, TDelta> builder,
//	Func<TState, TDelta, bool> condition) =>
//	new WhenBuilder<TState, TDelta>(condition);

//public class WhenBuilder<TState, TDelta> : Builder<TState, TDelta>
//{
//	private readonly Func<TState, TDelta, bool> Condition;

//	internal WhenBuilder(Func<TState, TDelta, bool> condition)
//	{
//		Condition = condition ?? throw new ArgumentNullException(nameof(condition));
//	}
//}

