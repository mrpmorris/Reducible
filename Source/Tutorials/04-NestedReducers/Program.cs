using Morris.Immutable;
using NestedReducers;
using System.Collections.Immutable;
using System.Text.Json;

// Create a reducer that adds an achievement to a student
// with a specific Id, only if they do not already
// have that achievement.
var studentAddAchievementReducer = Reducer
	.Given<Student, AddStudentAchievementAction>()
	.When((s, a) => s.Id == a.StudentId && !s.Achievements.Contains(a.Achievement))
	.Then((s, a) => s with { Achievements = s.Achievements.Add(a.Achievement) });

// Create a reducer for the school, which uses the previous reducer
// on each student in its `ImmutableArray<Student> Students` property
// to add an achievement to a student with a specific Id if they do not
// already have that achievement.
var schoolAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievementAction>()
	.WhenReducedBy(x => x.Students, studentAddAchievementReducer)
	.Then((school, students) => school with { Students = students });


var student1 = new Student(1, "Peter Morris");
var student2 = new Student(2, "Steven Cramer");

var allStudents = ImmutableArray.Create<Student>(student1, student2);
var school = new School(allStudents);

var grantAction = new AddStudentAchievementAction(2, "Smells");

Console.WriteLine($"Original state={JsonSerializer.Serialize(school)}");
for (int i = 0; i < 2; i++)
{
	(bool changed, school) = schoolAddAchievementReducer(school, grantAction);
	Console.ForegroundColor = changed ? ConsoleColor.Red : ConsoleColor.Green;
	Console.WriteLine($"Step={i + 1}, Changed={changed}, State={JsonSerializer.Serialize(school)}");
}
//	Output:
//		Original state={"Students":[{"Id":1,"Name":"Peter Morris","Achievements":[]},{"Id":2,"Name":"Steven Cramer","Achievements":[]}]}
//		Step=1, Changed=True, State={"Students":[{"Id":1,"Name":"Peter Morris","Achievements":[]},{"Id":2,"Name":"Steven Cramer","Achievements":["Smells"]}]}
//		Step=2, Changed=False, State={"Students":[{"Id":1,"Name":"Peter Morris","Achievements":[]},{"Id":2,"Name":"Steven Cramer","Achievements":["Smells"]}]}

Console.ForegroundColor = ConsoleColor.White;
Console.ReadLine();
