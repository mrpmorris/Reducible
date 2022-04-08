using Morris.Immutable;
using SimpleReducers;
using System.Text.Json;

// Create a simple unconditional reducer.
// The result should be (bool Changed, TState newState)
var counterIncrementCounterReducer = Reducer
	.Given<CounterState, IncrementCounterAction>()
	.Then((s, a) => (true, s with { Counter = s.Counter + a.Delta }));

var state = new CounterState(0);
var action = new IncrementCounterAction(1);

Console.WriteLine($"Original state={JsonSerializer.Serialize(state)}");
for (int i = 0; i < 3; i++)
{
	(bool changed, state) = counterIncrementCounterReducer(state, action);
	Console.WriteLine($"Step={i + 1}, Changed={changed}, State={JsonSerializer.Serialize(state)}");
}
//	Output:
//		Original state={"Counter":0}
//		Step=1, Changed=True, State={"Counter":1}
//		Step=2, Changed=True, State={"Counter":2}
//		Step=3, Changed=True, State={"Counter":3}

Console.ForegroundColor = ConsoleColor.White;
Console.ReadLine();

