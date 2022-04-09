using ConditionalReducers;
using Morris.Reducible;
using System.Text.Json;

// Create a conditional reducer.
// The result should be TState newState.
// The conditional reducer will change this to (bool Changed, TState newState)
// based on whether or not the condition triggered the reducer code.
var counterIncrementCounterReducer = Reducer
	.Given<CounterState, IncrementCounter>()
	.When((state, delta) => state.Counter < 2)
	.Then((state, delta) => state with { Counter = state.Counter + delta.Amount });

var state = new CounterState(0);
var delta = new IncrementCounter(1);

ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
Console.WriteLine($"Original state={JsonSerializer.Serialize(state, jsonOptions)}");
for (int i = 0; i < 3; i++)
{
	(bool changed, state) = counterIncrementCounterReducer(state, delta);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"\r\nStep={i + 1}, Changed={changed}\r\nState={JsonSerializer.Serialize(state, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();
