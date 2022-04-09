using Morris.Reducible;
using SimpleReducers;
using System.Text.Json;

// Create a simple unconditional reducer.
// The result should be (bool Changed, TState newState)
var counterIncrementCounterReducer = Reducer
	.Given<CounterState, IncrementCounterAction>()
	.Then((s, a) => (true, s with { Counter = s.Counter + a.Delta }));

var state = new CounterState(0);
var action = new IncrementCounterAction(1);

var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
ConsoleColor defaultColor = Console.ForegroundColor;
Console.WriteLine($"Original state={JsonSerializer.Serialize(state, jsonOptions)}");
for (int i = 0; i < 3; i++)
{
	(bool changed, state) = counterIncrementCounterReducer(state, action);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"\r\nStep={i + 1}, Changed={changed}\r\nState={JsonSerializer.Serialize(state, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();

