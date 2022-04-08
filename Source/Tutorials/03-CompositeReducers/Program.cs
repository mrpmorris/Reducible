using Morris.Immutable;
using System.Text.Json;

// Create a conditional reducer.
// If the value specified is even then update State.EvenValue
var evenValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => a.Value % 2 == 0 && s.EvenValue <= 4)
	.Then((s, a) => s with { EvenValue = a.Value });

// Create another conditional reducer.
// If the value specified is odd *and* less than 3 then update State.OddValue
var oddValueReducer = Reducer
	.Given<ValuesState, UpdateValuesAction>()
	.When((s, a) => a.Value % 2 != 0 && s.OddValue < 3)
	.Then((s, a) => s with { OddValue = a.Value });

// Now create a reducer that combines the two reducers.
var compositeReducer = Reducer.Combine(evenValueReducer, oddValueReducer);

var state = new ValuesState(EvenValue: 0, OddValue: 0);
for (int i = 1; i <= 6; i++)
{
	var action = new UpdateValuesAction(i);
	(bool changed, state) = compositeReducer(state, action);
	Console.WriteLine($"Step={i + 1}, Changed={changed}, State={JsonSerializer.Serialize(state)}");
}
//	Output:
//		Step=3, Changed=True, State={"EvenValue":2,"OddValue":1}
//		Step=4, Changed=True, State={"EvenValue":2,"OddValue":3}
//		Step=5, Changed=True, State={"EvenValue":4,"OddValue":3}
//		Step=6, Changed=False, State={"EvenValue":4,"OddValue":3}
//		Step=7, Changed=True, State={"EvenValue":6,"OddValue":3}
Console.ReadLine();


record ValuesState(int EvenValue, int OddValue);
record UpdateValuesAction(int Value);
