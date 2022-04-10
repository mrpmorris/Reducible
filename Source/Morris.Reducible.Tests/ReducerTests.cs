using Xunit;

namespace Morris.Reducible.Tests
{
	public class ReducerTests
    {
	    public record CounterState(int Count);
	    public record CountAction(int Delta);

		[Fact]
	    public void SimpleReducerTest()
	    {
			//Given
			var initialState = new CounterState(0);
			var action = new CountAction(1);
			var counterIncrementCounterReducer = Reducer
			 .Given<CounterState, CountAction>()
			 .Then((state, delta) => (true, state with { Count = state.Count + delta.Delta}));

			//When
			(bool changed, var counterState) = counterIncrementCounterReducer.Invoke(initialState, action);

			//Then
			Assert.True(changed);
			Assert.Equal(new CounterState(1), counterState);
	    }
	}
}
