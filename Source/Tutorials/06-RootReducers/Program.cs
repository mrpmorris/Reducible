using Morris.Immutable;
using System.Collections.Immutable;
using System.Text.Json;
using RootReducers;

#region Reducers from previous example
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
var schoolStudentsAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievementAction>()
	.WhenReducedBy(x => x.Students, studentAddAchievementReducer)
	.Then((school, students) => school with { Students = students });

// Create a reducer for the school, which uses the student reducer
// on its `Student HeadStudent` property
// to add an achievement if they have the correct Id and if they do not
// already have that achievement.
var schoolHeadStudentAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievementAction>()
	.WhenReducedBy(x => x.HeadStudent, studentAddAchievementReducer)
	.Then((school, headStudent) => school with { HeadStudent = headStudent });

// Create a combined reducer that takes School as a state,
// AddStudentAchievementAction as an action
// and returns new school state if any of the Students or the HeadStudent
// states were modified.
var schoolAddStudentAchievementReducer = Reducer.Combine(schoolHeadStudentAddAchievementReducer, schoolStudentsAddAchievementReducer);
#endregion

// Create a reducer that replaces the head student
// of the school, because Steven Cramer is too smelly.
var schoolChangeHeadStudentReducer = Reducer
	.Given<School, ChangeHeadStudentAction>()
	.When((s, a) => s.HeadStudent.Id != a.Student.Id)
	.Then((s, a) => s with { HeadStudent = a.Student });

var student1 = new Student(1, "Peter Morris");
var student2 = new Student(2, "Steven Cramer");

var allStudents = ImmutableArray.Create<Student>(student1, student2);
var school = new School(allStudents, HeadStudent: student2);

var grantAction = new AddStudentAchievementAction(2, "Smells");
var changeHeadStudentAction = new ChangeHeadStudentAction(student1);

Console.WriteLine($"Original state={JsonSerializer.Serialize(school)}");

// Now build a reducer that can handle both
// actions by allowing us to pass (TState, object)
var schoolReducer = Reducer.CreateBuilder<School>()
	.Add(schoolAddStudentAchievementReducer)
	.Add(schoolChangeHeadStudentReducer)
	.Build();

(bool changed, school) = schoolReducer(school, grantAction);
(changed, school) = schoolReducer(school, changeHeadStudentAction);

Console.WriteLine($"New state={JsonSerializer.Serialize(school)}");
//	Output:
//		Original state={"Students":[{"Id":1,"Name":"Peter Morris","Achievements":[]},{"Id":2,"Name":"Steven Cramer","Achievements":[]}],"HeadStudent":{"Id":2,"Name":"Steven Cramer","Achievements":[]}}
//		New state={"Students":[{"Id":1,"Name":"Peter Morris","Achievements":[]},{"Id":2,"Name":"Steven Cramer","Achievements":["Smells"]}],"HeadStudent":{"Id":1,"Name":"Peter Morris","Achievements":[]}}

Console.ForegroundColor = ConsoleColor.White;
Console.ReadLine();
