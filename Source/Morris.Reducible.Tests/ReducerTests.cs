using Xunit;

namespace Morris.Reducible.Tests
{
	public class ReducerTests
    {
	    private record CounterState(int Counter);
	    private record IncrementCounter(int Amount);


		[Fact]
	    public void WhenSimpleReducerInvoked_ThenStateIsUpdated()
	    {
			//Given
			var initialState = new CounterState(0);
			var action = new IncrementCounter(1);
			var counterIncrementCounterReducer = Reducer
			 .Given<CounterState, IncrementCounter>()
			 .Then((state, delta) => (true, state with { Counter = state.Counter + delta.Amount}));

			//When
			(bool changed, var counterState) = counterIncrementCounterReducer(initialState, action);

			//Then
			Assert.True(changed);
			Assert.Equal(new CounterState(1), counterState);
	    }	
	    
	    [Fact]
	    public void WhenConditionalReducerInvokedWithPassingCondition_ThenStateIsUpdated()
	    {
			//Given
			var initialState = new CounterState(0);
			var action = new IncrementCounter(1);
			var counterIncrementCounterReducer = Reducer
				.Given<CounterState, IncrementCounter>()
				.When((state, delta) => state.Counter < 2)
				.Then((state, delta) => state with { Counter = state.Counter + delta.Amount });

			//When
			(bool changed, var counterState) = counterIncrementCounterReducer(initialState, action);

			//Then
			Assert.True(changed);
			Assert.Equal(new CounterState(1), counterState);
	    }

	    [Fact]
	    public void WhenConditionalReducerInvokedWithFailingCondition_ThenStateIsNotUpdated()
	    {
		    //Given
		    var initialState = new CounterState(0);
		    var action = new IncrementCounter(1);
		    var counterIncrementCounterReducer = Reducer
			    .Given<CounterState, IncrementCounter>()
			    .When((state, delta) => state.Counter > 2)
			    .Then((state, delta) => state with { Counter = state.Counter + delta.Amount });

		    //When
		    (bool changed, var counterState) = counterIncrementCounterReducer(initialState, action);

		    //Then
		    Assert.False(changed);
		    Assert.Equal(new CounterState(0), counterState);
	    }

	    [Fact]
	    public void WhenCombinedReducerInvoked_ThenStateIsUpdated()
	    {
		    //Given
		    var initialState = new CounterState(0);
		    var action = new IncrementCounter(1);
		    var counterIncrementCounterReducer1 = Reducer
			    .Given<CounterState, IncrementCounter>()
			    .Then((state, delta) => (true, state with { Counter = state.Counter + delta.Amount }));

		    var counterIncrementCounterReducer2 = Reducer
			    .Given<CounterState, IncrementCounter>()
			    .Then((state, delta) => (true, state with { Counter = state.Counter + delta.Amount }));
		    var combinedReducer = Reducer.Combine(counterIncrementCounterReducer1, counterIncrementCounterReducer2);

			//When
			(bool changed, var counterState) = combinedReducer(initialState, action);

		    //Then
		    Assert.True(changed);
		    Assert.Equal(new CounterState(2), counterState);
	    }
	}
}
