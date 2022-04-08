using System.Collections.Immutable;

namespace RootReducers;

record School(ImmutableArray<Student> Students, Student HeadStudent);
record Student(int Id, string Name)
{
	public ImmutableHashSet<string> Achievements { get; init; } = ImmutableHashSet.Create<string>(StringComparer.InvariantCultureIgnoreCase);
}

record AddStudentAchievementAction(int StudentId, string Achievement);
record ChangeHeadStudentAction(Student Student);