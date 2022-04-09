using System.Collections.Immutable;

namespace PolyMorphicReducers;

record School(ImmutableArray<Student> Students, Student HeadStudent);
record Student(int Id, string Name)
{
	public ImmutableHashSet<string> Achievements { get; init; } = ImmutableHashSet.Create<string>(StringComparer.InvariantCultureIgnoreCase);
}

record AddStudentAchievement(int StudentId, string Achievement);
record ChangeHeadStudent(Student Student);