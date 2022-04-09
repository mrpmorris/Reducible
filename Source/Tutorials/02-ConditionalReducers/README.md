# Reducible
## Conditional reducers
### Goal
This tutorial will demonstrate how to create a reducer that will only reduce the
`Delta` into the `State` if a precondition is met by using a `When` clause.

Use of the `When` clause means your reduce operation no longer needs to return
the `bool changed` value. If the condition in the `When` clause is not met
then the reducer will return `(false, originalState)`, if the clause is met
then the reduce code will be executed and will return `(true, newState)`.

### Instructions

Create a new console app with the following `State` and `Delta` classes.

```c#
record CounterState(int Counter);
record IncrementCounter(int Amount);
```

We can now create a reducer that combines the state `CounterState` and
the delta `IncrementCounter` to give a new `CounterState`.

```c#
var counterIncrementCounterReducer = Reducer
  .Given<CounterState, IncrementCounter>()
  .When((state, delta) => state.Counter < 2)
  .Then((state, delta) =>
    state with { Counter = state.Counter + delta.Amount });
```

* **Given** specifies which types can be used as inputs for the reducer.
* **When** specifies the whether the reducer needs to be executed or not.
* **Then** expects a function that receives a tuple `(state, delta)`
  and returns the new `State`.

Calling the function returned will again return a **Reducer.Result**, which contains
both the `bool changed` and `TState state`.

The state in this example changes like so...
```
Original state={ "Counter": 0 }
Step=1, Changed=True, State={ "Counter": 1 }
Step=2, Changed=True, State={ "Counter": 2 }
Step=3, Changed=False, State={ "Counter": 2 }
```

