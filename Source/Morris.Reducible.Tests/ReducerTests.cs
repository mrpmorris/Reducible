using Xunit;

namespace Morris.Reducible.Tests
{
	public class ReducerTests
    {
	    record CounterState(int Counter);
	    record IncrementCounter(int Amount);

		[Fact]
	    public void SimpleReducerTest()
	    {
			//Given
			var initialState = new CounterState(0);
			var action = new IncrementCounter(1);
			var counterIncrementCounterReducer = Reducer
			 .Given<CounterState, IncrementCounter>()
			 .Then((state, delta) => (true, state with { Counter = state.Counter + delta.Amount}));

			//When
			(bool changed, var counterState) = counterIncrementCounterReducer.Invoke(initialState, action);

			//Then
			Assert.True(changed);
			Assert.Equal(new CounterState(1), counterState);
	    }	
	    
	    [Fact]
	    public void ConditionalReducerModifysStateWhenConditionPasses()
	    {
			//Given
			var initialState = new CounterState(0);
			var action = new IncrementCounter(1);
			var counterIncrementCounterReducer = Reducer
				.Given<CounterState, IncrementCounter>()
				.When((state, delta) => state.Counter < 2)
				.Then((state, delta) => state with { Counter = state.Counter + delta.Amount });

			//When
			(bool changed, var counterState) = counterIncrementCounterReducer.Invoke(initialState, action);

			//Then
			Assert.True(changed);
			Assert.Equal(new CounterState(1), counterState);
	    }

	    [Fact]
	    public void ConditionalReducerDoesNotModifyStateWhenConditionFails()
	    {
		    //Given
		    var initialState = new CounterState(0);
		    var action = new IncrementCounter(1);
		    var counterIncrementCounterReducer = Reducer
			    .Given<CounterState, IncrementCounter>()
			    .When((state, delta) => state.Counter > 2)
			    .Then((state, delta) => state with { Counter = state.Counter + delta.Amount });

		    //When
		    (bool changed, var counterState) = counterIncrementCounterReducer.Invoke(initialState, action);

		    //Then
		    Assert.False(changed);
		    Assert.Equal(new CounterState(0), counterState);
	    }
	}
}
