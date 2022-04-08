using System.Collections.Immutable;

namespace NestedReducers;

record School(ImmutableArray<Student> Students);
record Student (int Id, string Name)
{
	public ImmutableHashSet<string> Achievements { get; init; } = ImmutableHashSet.Create<string>(StringComparer.InvariantCultureIgnoreCase);
}

record AddStudentAchievementAction(int StudentId, string Achievement);