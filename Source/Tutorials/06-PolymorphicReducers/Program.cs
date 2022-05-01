using Morris.Reducible;
using System.Collections.Immutable;
using System.Text.Json;
using PolyMorphicReducers;

#region Reducers from previous example
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
var schoolStudentsAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievement>()
	.WhenReducedBy(x => x.Students, studentAddAchievementReducer)
	.Then((school, students) => school with { Students = students });

// Create a reducer for the school, which uses the student reducer
// on its `Student HeadStudent` property
// to add an achievement if they have the correct Id and if they do not
// already have that achievement.
var schoolHeadStudentAddAchievementReducer = Reducer
	.Given<School, AddStudentAchievement>()
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
	.Given<School, ChangeHeadStudent>()
	.When((school, delta) => school.HeadStudent.Id != delta.Student.Id)
	.Then((student, delta) => student with { HeadStudent = delta.Student });

// Now build a reducer that can handle both
// Delta types by allowing us to pass `TState` + `object`
var schoolReducer = Reducer.CreateCompositeBuilder<School>()
	.Add(schoolAddStudentAchievementReducer)
	.Add(schoolChangeHeadStudentReducer)
	.Build();


var student1 = new Student(1, "Peter Morris");
var student2 = new Student(2, "Steven Cramer");

var allStudents = ImmutableArray.Create<Student>(student1, student2);
var school = new School(allStudents, HeadStudent: student2);

var addAchievementDelta = new AddStudentAchievement(2, "Smells");
var changeHeadStudentDelta = new ChangeHeadStudent(student1);


ConsoleColor defaultColor = Console.ForegroundColor;
var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

DisplayState(step: 0, school, false, jsonOptions, defaultColor);

(bool changed, school) = schoolReducer(school, addAchievementDelta);
DisplayState(step: 1, school, changed, jsonOptions);

(changed, school) = schoolReducer(school, changeHeadStudentDelta);
DisplayState(step: 2, school, changed, jsonOptions);

(changed, school) = schoolReducer(school, "We don't have a reducer for a simple string delta, so this does nothing");
DisplayState(step: 3, school, changed, jsonOptions);

Console.ForegroundColor = defaultColor;
Console.ReadLine();

void DisplayState(int step, School state, bool changed, JsonSerializerOptions jsonOptions, ConsoleColor? forcedColor = null)
{
	Console.ForegroundColor = forcedColor ?? (changed ? ConsoleColor.Cyan : defaultColor);
	Console.WriteLine($"\r\nStep={step}, Changed={changed},\r\nState={JsonSerializer.Serialize(school, jsonOptions)}");
}
