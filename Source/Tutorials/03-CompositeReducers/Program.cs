using CompositeReducers;
using Morris.Reducible;
using System.Text.Json;

var IsEven= (int value) => value % 2 == 0;
var IsOdd = (int value) => value % 2 != 0;

// Create a conditional reducer
// When the value specified is even *and* less than 3
// then update State.EvenValue
var evenValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => IsEven(a.Value) && a.Value < 3)
	.Then((s, a) => s with { EvenValue = a.Value });

// Create a conditional reducer
// When the value specified is odd *and* less than 3
// then update State.OddValue
var oddValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => IsOdd(a.Value) && a.Value < 3)
	.Then((s, a) => s with { OddValue = a.Value });

// Now create a reducer that combines the two reducers.
var compositeReducer = Reducer.Combine(evenValueReducer, oddValueReducer);

var state = new ValuesState(EvenValue: 0, OddValue: 0);

ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
Console.WriteLine($"Original state={JsonSerializer.Serialize(state, jsonOptions)}");
for (int i = 1; i < 5; i++)
{
	var action = new UpdateValuesAction(i);
	(bool changed, state) = compositeReducer(state, action);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"\r\nStep={i + 1}, Changed={changed}\r\nState={JsonSerializer.Serialize(state, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();
