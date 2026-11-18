namespace GradeBook;

// Reads and writes students in CSV files: firstname,lastname,gpa, with that header as the first line.
//
// Requirements:
// - Read(path) returns the students in the file, in the file's order.
//   - The header line is skipped, and so are blank lines.
//   - Spaces around a value are ignored: "Emily, Wilson, 3.91" is Emily Wilson with a GPA of 3.91.
//   - A line that doesn't have exactly 3 values, or whose GPA isn't a number, is skipped and logged to the
//     error log as "{file name} line {line number}: can't read '{line}'", such as
//     "student_grades.csv line 7: can't read 'David,Jones'". Line numbers count from 1, header included.
//   - If the file isn't there, Read logs "{file name} not found" and returns an empty list.
// - WriteHonorRoll(path, students, minimumGpa) writes the header, then every student whose GPA is at
//   least minimumGpa, in the list's order, with the GPA to two decimal places, such as "Emily,Wilson,3.91".
//   It replaces any file already there.
// - AddStudent(path, student) adds the student as one line at the end of the file, with the GPA to two
//   decimal places. If the file isn't there yet, it's created with the header first.
//
// Step 5, for an import that must be all or nothing:
// - ReadAll(path) returns the students in the file, like Read, but stops at the first line it can't read.
//   - The header line is skipped, and so are blank lines. Each other line becomes a Student through
//     StudentLine.Parse(line), which throws a FormatException when it can't read the line.
//   - ReadAll catches that FormatException and throws a new FormatException instead, with:
//     - the message "{file name} line {line number}: can't read '{line}'", such as
//       "student_grades.csv line 7: can't read 'David,Jones'" (line numbers count from 1, header included);
//     - the original exception as its InnerException;
//     - the line number, an int, in its Data under the name "LineNumber".
//   - ReadAll logs nothing and doesn't catch a missing file.
// - ImportHonorRoll(sourcePath, honorRollPath, minimumGpa) reads every student with ReadAll, then writes
//   the honor roll with WriteHonorRoll and returns true.
//   - If ReadAll throws a FormatException, ImportHonorRoll catches it, logs
//     "Honor roll not imported: {the exception's message}", writes no file, and returns false.
public class GradeFile
{
    private const string Header = "firstname,lastname,gpa";

    private readonly LogService errorLog;

    public GradeFile(LogService errorLog)
    {
        this.errorLog = errorLog;
    }

    public List<Student> Read(string path)
    {
        List<Student> students = new List<Student>();
        return students;
    }

    public void WriteHonorRoll(string path, List<Student> students, decimal minimumGpa)
    {
    }

    public void AddStudent(string path, Student student)
    {
    }

    public List<Student> ReadAll(string path)
    {
        List<Student> students = new List<Student>();
        return students;
    }

    public bool ImportHonorRoll(string sourcePath, string honorRollPath, decimal minimumGpa)
    {
        return false;
    }
}
