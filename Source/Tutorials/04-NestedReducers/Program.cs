using Morris.Reducible;
using NestedReducers;
using System.Collections.Immutable;
using System.Text.Json;

// Create a reducer that adds an achievement to a student
// with a specific Id, only if they do not already
// have that achievement.
var studentAddAchievementReducer = Reducer
	.Given<Student, AddStudentAchievement>()
	.When((student, delta) => student.Id == delta.StudentId && !student.Achievements.Contains(delta.Achievement))
	.Then((student, delta) => student with { Achievements = student.Achievements.Add(delta.Achievement) });

// Create a reducer for the school, which uses the previous reducer
// on each student in its `ImmutableArray<Student> Students` property
// to add an achievement to a student with a specific Id if they do not
// already have that achievement.
var schoolAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievement>()
	.WhenReducedBy(x => x.Students, studentAddAchievementReducer, (school, students) => school with { Students = students })
	.Then((school, delta) => school);


var student1 = new Student(1, "Peter Morris");
var student2 = new Student(2, "Steven Cramer");

var allStudents = ImmutableArray.Create<Student>(student1, student2);
var school = new School(allStudents);

var addAchievementDelta = new AddStudentAchievement(2, "Smells");

ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
Console.WriteLine($"Original state={JsonSerializer.Serialize(school, jsonOptions)}");
for (int i = 0; i < 2; i++)
{
	(bool changed, school) = schoolAddAchievementReducer(school, addAchievementDelta);
	Console.ForegroundColor = changed ? ConsoleColor.Cyan : defaultColor;
	Console.WriteLine($"Step={i + 1}, Changed={changed}, State={JsonSerializer.Serialize(school, jsonOptions)}");
}

Console.ForegroundColor = defaultColor;
Console.ReadLine();
