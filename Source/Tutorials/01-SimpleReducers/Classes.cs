namespace SimpleReducers;

record CounterState(int Counter);
record IncrementCounter(int Amount);
record IncrementCounterWithFactor(IncrementCounter increment, int factor);