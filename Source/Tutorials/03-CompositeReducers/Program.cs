using CompositeReducers;
using Morris.Reducible;
using System.Text.Json;

var IsEven = (int value) => value % 2 == 0;
var IsOdd = (int value) => value % 2 != 0;

// Create a conditional reducer
// When the value specified is even *and* no greater than 2
// then update State.EvenValue
var evenValueReducer = Reducer
	.Given<ValuesState, UpdateValues>()
	.When((values, delta) => IsEven(delta.Value) && delta.Value <= 2)
	.Then((values, delta) => values with { EvenValue = delta.Value });

// Create a conditional reducer
// When the value specified is odd *and* no greater than 3
// then update State.OddValue
var oddValueReducer = Reducer
	.Given<ValuesState, UpdateValues>()
	.When((values, delta) => IsOdd(delta.Value) && delta.Value <= 3)
	.Then((values, delta) => values with { OddValue = delta.Value });

// Now create a reducer that combines the two reducers.
var compositeReducer = Reducer.Combine(evenValueReducer, oddValueReducer);

var state = new ValuesState(EvenValue: 0, OddValue: 0);

ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
Console.WriteLine($"Original state={JsonSerializer.Serialize(state, jsonOptions)}");
for (int i = 1; i < 5; i++)
{
	var delta = new UpdateValues(i);
	(bool changed, state) = compositeReducer(state, delta);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"\r\nStep={i}, Changed={changed}\r\nState={JsonSerializer.Serialize(state, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();
