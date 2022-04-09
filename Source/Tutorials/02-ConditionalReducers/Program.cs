using ConditionalReducers;
using Morris.Reducible;
using System.Text.Json;

// Create a conditional reducer.
// The result should be TState newState.
// The conditional reducer will change this to (bool Changed, TState newState)
// based on whether or not the condition triggered the reducer code.
var counterIncrementCounterReducer = Reducer
	.Given<CounterState, IncrementCounterAction>()
	.When((s, a) => s.Counter < 2)
	.Then((s, a) => s with { Counter = s.Counter + a.Delta });

var state = new CounterState(0);
var action = new IncrementCounterAction(1);

ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
Console.WriteLine($"Original state={JsonSerializer.Serialize(state, jsonOptions)}");
for (int i = 0; i < 3; i++)
{
	(bool changed, state) = counterIncrementCounterReducer(state, action);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"\r\nStep={i + 1}, Changed={changed}\r\nState={JsonSerializer.Serialize(state, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();
