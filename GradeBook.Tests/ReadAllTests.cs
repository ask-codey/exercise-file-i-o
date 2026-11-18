namespace GradeBook.Tests;

// Step 5: ReadAll and ImportHonorRoll, for imports that must be all or nothing.
[TestClass]
public class ReadAllTests
{
    private string folder = "";
    private LogService errorLog = null!;
    private GradeFile grades = null!;

    [TestInitialize]
    public void MakeFolder()
    {
        folder = Path.Combine(Path.GetTempPath(), "GradeBookTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(folder);
        errorLog = new LogService(Path.Combine(folder, "errors"), new FakeClock(new DateTime(2026, 3, 14, 9, 0, 0)));
        grades = new GradeFile(errorLog);
    }

    [TestCleanup]
    public void RemoveFolder()
    {
        if (Directory.Exists(folder))
        {
            Directory.Delete(folder, recursive: true);
        }
    }

    private string WriteFile(params string[] lines)
    {
        string path = Path.Combine(folder, "grades.csv");
        File.WriteAllLines(path, lines);
        return path;
    }

    private string[] ErrorLogLines()
    {
        string path = Path.Combine(folder, "errors", "20260314.log");
        return File.Exists(path) ? File.ReadAllLines(path) : new string[0];
    }

    [TestMethod]
    public void ReadAll_GoodFile_ReturnsEveryStudentAfterTheHeaderAndSkipsBlankLines()
    {
        string path = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "", "John,Smith,2.55");

        List<Student> students = grades.ReadAll(path);

        Assert.HasCount(2, students);
        Assert.AreEqual("Emily", students[0].FirstName);
        Assert.AreEqual(3.91m, students[0].Gpa);
        Assert.AreEqual("John", students[1].FirstName);
    }

    [TestMethod]
    public void ReadAll_GpaNotANumber_ThrowsAFormatExceptionThatNamesTheLine()
    {
        string path = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "Anna,Williams,n/a");

        FormatException ex = Assert.ThrowsExactly<FormatException>(() => grades.ReadAll(path));

        Assert.AreEqual("grades.csv line 3: can't read 'Anna,Williams,n/a'", ex.Message);
    }

    [TestMethod]
    public void ReadAll_GpaNotANumber_KeepsTheOriginalExceptionAsInnerException()
    {
        string path = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "Anna,Williams,n/a");

        FormatException ex = Assert.ThrowsExactly<FormatException>(() => grades.ReadAll(path));

        Assert.IsInstanceOfType<FormatException>(ex.InnerException);
        Assert.AreEqual("The input string 'n/a' was not in a correct format.", ex.InnerException.Message);
    }

    [TestMethod]
    public void ReadAll_MissingValue_PutsTheLineNumberInData()
    {
        string path = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "Jane,Garcia,3.96", "David,Jones");

        FormatException ex = Assert.ThrowsExactly<FormatException>(() => grades.ReadAll(path));

        Assert.AreEqual("grades.csv line 4: can't read 'David,Jones'", ex.Message);
        Assert.AreEqual(4, ex.Data["LineNumber"]);
    }

    [TestMethod]
    public void ImportHonorRoll_GoodFile_WritesTheHonorRollAndReturnsTrue()
    {
        string source = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "John,Smith,2.55");
        string honorRoll = Path.Combine(folder, "honor_roll.csv");

        bool imported = grades.ImportHonorRoll(source, honorRoll, 3.5m);

        Assert.IsTrue(imported);
        CollectionAssert.AreEqual(new[] { "firstname,lastname,gpa", "Emily,Wilson,3.91" }, File.ReadAllLines(honorRoll));
        Assert.IsEmpty(ErrorLogLines());
    }

    [TestMethod]
    public void ImportHonorRoll_BadLine_LogsTheMessageWritesNothingAndReturnsFalse()
    {
        string source = WriteFile("firstname,lastname,gpa", "Emily,Wilson,3.91", "Anna,Williams,n/a");
        string honorRoll = Path.Combine(folder, "honor_roll.csv");

        bool imported = grades.ImportHonorRoll(source, honorRoll, 3.5m);

        Assert.IsFalse(imported);
        Assert.IsFalse(File.Exists(honorRoll));
        CollectionAssert.AreEqual(
            new[] { "2026-03-14 09:00:00 Honor roll not imported: grades.csv line 3: can't read 'Anna,Williams,n/a'" },
            ErrorLogLines());
    }
}
