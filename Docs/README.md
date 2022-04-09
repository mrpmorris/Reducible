# Reducible - documentation

## Reducer pattern

Made popular by the JavaScript Redux library, and various other Flux pattern frameworks,
the Reducer pattern is a very simple concept. You can take two states, and reduce them into one new state.

![](./../Images/reducer-explanation-1.jpg)

The states being reduced don't need to be of the same type / structure.

![](./../Images/reducer-explanation-2.jpg)

If you consider the class type of the state, you can even consider an object with no properties as a state
that may be combined.

![](./../Images/reducer-explanation-3.jpg)

The latter is a perfect example of the Flux pattern, where an existing state is reduced with a `Delta` state (action)
to produce a new state.

The Delta state (called an action in Flux) can have any number of properties (e.g. `ClosedUtc` to record the date/time
it was closed), or no properties at all and the reducer determines the change based on the object type of the `Delta` state alone.

![](./../Images/reducer-explanation-4.jpg)

**Reducer rules**
* Reducers should not modify existing state; read-only state is better, but not essential.
* Reducers should be pure functions; meaning that the output should be derived from the input only,
  and not any additional sources such as databases, API calls, or the current Date/Time.
* Reducers should produce new state as its output.

## Tutorials

Read through the following tutorials in order to get a better idea of how this
library helps to achieve your goals, and what kind of features are available to
simplify the task.

* [Simple reducers](../Source/Tutorials/01-SimpleReducers/README.md)
* [Conditional reducers](../Source/Tutorials/02-ConditionalReducers/README.md)
* [Composite reducers](../Source/Tutorials/03-CompositeReducers/README.md)
* [Nested reducers](../Source/Tutorials/04-NestedReducers/README.md)
* [Composite nested reducers](../Source/Tutorials/05-CompositeNestedReducers/README.md)
* [Polymorphic reducers](../Source/Tutorials/06-PolymorphicReducers/README.md)

