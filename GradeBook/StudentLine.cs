namespace GradeBook;

// Turns one line of a grade file into a Student. It's finished.
//
// Parse("Emily, Wilson, 3.91") is Emily Wilson with a GPA of 3.91: spaces around a value are ignored.
// Parse throws a FormatException when the line doesn't have exactly 3 values, or its GPA isn't a number.
public static class StudentLine
{
    public static Student Parse(string line)
    {
        string[] columns = line.Split(',');
        if (columns.Length != 3)
        {
            throw new FormatException($"Expected 3 values but found {columns.Length}.");
        }

        decimal gpa = decimal.Parse(columns[2].Trim());
        return new Student(columns[0].Trim(), columns[1].Trim(), gpa);
    }
}
