# Reducible
![](./../../../Images/small-logo.png)
## Composite reducers
### Goal
This tutorial will demonstrate how take multiple reducers that operate on
the same `TState` and `TDelta`, and combine them into a single reducer.

The purpose of creating a composite reducer is to simplify execution. Instead of
having to call a series of reducers, we can compose them into a single function.
This means that anywhere we need to execute this call, we can ensure none of the
relevant reducers are accidentally left out of the process.

### Instructions

Create a new console app with the following `State` and `Delta` classes, and the following helper routines for determining
if a number is odd or even.

```c#
record ValuesState(int EvenValue, int OddValue);
record UpdateValues(int Value);

var IsEven = (int value) => value % 2 == 0;
var IsOdd = (int value) => value % 2 != 0;
```

#### Requirement 1
Our first requirement is to create a conditional reducer that only updates the `EvenValue` if `Delta.Value`
is an even number, and also if that value is no greater than `3`.

```c#
var evenValueReducer = Reducer
  .Given<ValuesState, UpdateValues>()
  .When((values, delta) => IsEven(delta.Value) && delta.Value <= 2)
  .Then((values, delta) => values with { EvenValue = delta.Value });
```

* **Given** specifies which types can be used as inputs for the reducer.
* **When** specifies the reducer should only execute if the input is an even number and no greater than 2.
* **Then** takes the current state and the delta, and returns a new state with `EvenValue` updated.

Now write the following code to execute that reducer.

```c#
var state = new ValuesState(EvenValue: 0, OddValue: 0);

for (int i = 1; i < 5; i++)
{
  var delta = new UpdateValues(i);
  (bool changed, state) = evenValueReducer  (state, delta);
}
```

So far, the state changes like so...

```
Original state={ "EvenValue": 0, "OddValue": 0 }
Step=1, Changed=False, State={ "EvenValue": 0, "OddValue": 0 }
Step=2, Changed=True, State={ "EvenValue": 2, "OddValue": 0 }
Step=3, Changed=False, State={ "EvenValue": 2, "OddValue": 0 }
Step=5, Changed=False, State={ "EvenValue": 2, "OddValue": 0 }
```

#### Requirement 2
Our next requirement is to do something similar for the `OddValue` state. Rather than complicating the existing reducer we
can write a new reducer with the single purpose of recording odd numbers.

```c#
var oddValueReducer = Reducer
  .Given<ValuesState, UpdateValues>()
  .When((values, delta) => IsOdd(delta.Value) && delta.Value <= 3)
  .Then((values, delta) => values with { OddValue = delta.Value });
```

* **Given** specifies which types can be used as inputs for the reducer.
* **When** specifies the reducer should only execute if the input is an odd number and no greater than 3.
* **Then** takes the current state and the delta, and returns a new state with `OddValue` updated.

Now change the test code to execute both reducers.

```c#
for (int i = 1; i < 5; i++)
{
  var delta = new UpdateValues(i);
  (bool changed, state) = evenValueReducer(state, delta);
  (changed, state) = oddValueReducer(state, delta);
}
```

Now the two reducers together change the state like so...

```
Original state={ "EvenValue": 0, "OddValue": 0 }
Step=1, Changed=True, State={ "EvenValue": 0, "OddValue": 1 }
Step=2, Changed=False, State={ "EvenValue": 2, "OddValue": 1 }
Step=3, Changed=True, State={ "EvenValue": 2, "OddValue": 3 }
Step=4, Changed=False, State={ "EvenValue": 2, "OddValue": 3 }
```

#### Requirement 3
We might need to use this pattern in many places in our application. We don't want to run the risk of forgetting to call
one of these reducers in one of the places we implement the pattern, but we also don't want to recreate the original
reducer in a more complex form.

Instead, we can let ***Reducible*** combine the two reducers into a single reducer automatically.

Create a new reducer which is a composite of `oddValueReducer` and `evenValueReducer`, and then update the consumin code
like so...

```c#
var compositeReducer = Reducer.Combine(evenValueReducer, oddValueReducer);

for (int i = 1; i < 5; i++)
{
  var delta = new UpdateValues(i);
  (bool changed, state) = compositeReducer(state, delta);
}
```

Finally, the composite reducer changes the state identically to it did when we manually called both reducers.

```
Original state={ "EvenValue": 0, "OddValue": 0 }
Step=1, Changed=True, State={ "EvenValue": 0, "OddValue": 1 }
Step=2, Changed=False, State={ "EvenValue": 2, "OddValue": 1 }
Step=3, Changed=True, State={ "EvenValue": 2, "OddValue": 3 }
Step=4, Changed=False, State={ "EvenValue": 2, "OddValue": 3 }
```

Note that the composite reducer returns `false` when the delta `UpdateValues(4)` state is reduced. This is because both the
`oddNumberReducer` and `evenNumberReducer` that it composes into a single reducer have ***When*** clauses that prevent
them from reducing values as high as `4`.

### Summary
Combining reducers allows us to keep our individual reducers small and focused on a
single goal, whilst allowing for complex state changes.
