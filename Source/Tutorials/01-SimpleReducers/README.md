# Reducible
## Simple reducers
### Goal
This tutorial will demonstrate how to create a reducer that will combine
existing state `CounterState` and apply a `Delta` state.

The `Delta` state is an instance of a class named `IncrementCounter` and has
a single property `Amount` which is an integer.

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
  .Then((state, delta) =>
    (
      true,
      state with { Counter = state.Counter + delta.Amount }
    ));
```

* **Given** specifies which types can be used as inputs for the reducer.
* **Then** expects a function that receives a tuple `(state, delta)`
  and returns a tuple `(bool, state)`.

The `bool` value is a flag that indicates whether the reducer altered the state. See
**Reducer.Result** below.

The `state` value is the new state, which is a combination of the original state and
the delta - "reduced" from two states into one.


```c#
var state = new CounterState(0);
var delta = new IncrementCounter(1);

// Apply the delta to the state 3 times
for (int i = 0; i < 3; i++)
{
	(bool changed, state) = counterIncrementCounterReducer(state, delta);
	Console.WriteLine($"\r\nStep={i + 1}, Changed={changed}\r\nState={JsonSerializer.Serialize(state)}");
}

Console.ReadLine();
```

### Reducer.Result

In addition to the new state, ***Reducible*** reducers also return a `bool` flag
to indicate whether the reducer altered the state.

Although it is not used in this example, it is a very important part of the pattern that
***Reducible*** uses when making a composite / tree of reducers. It allows a reducer at
the very top of a complex tree of reducers know whether or not for optimisation purposes
it can simply return the original state when unmodified.


