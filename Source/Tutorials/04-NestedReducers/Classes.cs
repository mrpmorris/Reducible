using System.Collections.Immutable;

namespace NestedReducers;

record Person (string Name);
record Classroom(string Subject, ImmutableArray<Person> Teachers, ImmutableArray<Person> Students);