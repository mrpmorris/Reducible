using CompositeReducers;
using Morris.Immutable;
using System.Text.Json;

var IsEven= (int value) => value % 2 == 0;
var IsOdd = (int value) => value % 2 != 0;

// Create a conditional reducer.
// If the value specified is even then update State.EvenValue
var evenValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => IsEven(a.Value))
	.Then((s, a) => s with { EvenValue = a.Value });

// Create another conditional reducer.
// If the value specified is odd *and* less than 3 then update State.OddValue
var oddValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => IsOdd(a.Value) && a.Value < 3)
	.Then((s, a) => s with { OddValue = a.Value });

// Now create a reducer that combines the two reducers.
var compositeReducer = Reducer.Combine(evenValueReducer, oddValueReducer);

var state = new ValuesState(EvenValue: 0, OddValue: 0);
for (int i = 1; i <= 6; i++)
{
	var action = new UpdateValuesAction(i);
	(bool changed, state) = compositeReducer(state, action);
	Console.ForegroundColor = changed ? ConsoleColor.Red : ConsoleColor.Green;
	Console.WriteLine($"Step={i + 1}, Changed={changed}, State={JsonSerializer.Serialize(state)}");
}
//	Output:
//		Step=2, Changed=True, State={"EvenValue":0,"OddValue":1}
//		Step=3, Changed=True, State={"EvenValue":2,"OddValue":1}
//		Step=4, Changed=False, State={"EvenValue":2,"OddValue":1}
//		Step=5, Changed=True, State={"EvenValue":4,"OddValue":1}
//		Step=6, Changed=False, State={"EvenValue":4,"OddValue":1}
//		Step=7, Changed=True, State={"EvenValue":6,"OddValue":1}

Console.ReadLine();
