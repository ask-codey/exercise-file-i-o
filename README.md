# Exercise: File I/O

Exercise code for the lesson **Exercise: File I/O** in the C# Full-Stack Bootcamp on [ask-codey.com](https://www.ask-codey.com).

A school keeps its students' grades in a CSV file exported from a spreadsheet, and the file has a few mistakes in it. You'll write the code that reads it, logs the lines it can't read, writes an honor roll, and adds new students.

## What's here

- `GradeBook/data/student_grades.csv`: the school's file, mistakes and all. It's copied next to the program when it builds.
- `GradeBook/Student.cs`, `IClock.cs`, `SystemClock.cs`, `StudentLine.cs`: ready to use. `StudentLine.Parse` turns one line into a `Student`, and throws a `FormatException` when it can't.
- `GradeBook/LogService.cs`: writes time-stamped entries to a log file per day. For you to finish.
- `GradeBook/GradeFile.cs`: reads and writes the CSV files. For you to finish.
- `GradeBook.Tests`: tests for both, which work in a temporary folder and use a fake clock. You don't need to change these.

The requirements are in the comment at the top of each file you finish.

## What to do

1. **`LogService`:** `CurrentFilePath`, then `Log`.
2. **`GradeFile.Read`:** skip the header and blank lines, skip and log the bad lines, and log a missing file.
3. **`GradeFile.WriteHonorRoll`:** the header, then the students at or above the minimum GPA.
4. **`GradeFile.AddStudent`:** one line at the end, with the header first in a new file.
5. **`GradeFile.ReadAll` and `ImportHonorRoll`:** an import that's all or nothing. `ReadAll` reads every line with `StudentLine.Parse` and, at the first line it can't read, throws a new `FormatException` with the message `{file name} line {line number}: can't read '{line}'` (such as `student_grades.csv line 7: can't read 'David,Jones'`), the original exception as its `InnerException`, and the line number in its `Data` under `"LineNumber"`. `ImportHonorRoll` calls `ReadAll`, then `WriteHonorRoll`, and returns `true`; when `ReadAll` throws a `FormatException`, it catches it there, logs `Honor roll not imported: ` followed by the exception's message, writes no file, and returns `false`. Its tests are in `ReadAllTests.cs`.

Run the program with **Ctrl+F5**, and the tests with **Test > Run All Tests** (or `dotnet test`).

## Tips

- Do `LogService` first: `Read` uses it, and its tests are the simplest.
- Dates: `{clock.Now:yyyyMMdd}` for the file name, `{clock.Now:yyyy-MM-dd HH:mm:ss}` for an entry. Capital `HH` is the 24-hour clock.
- `File.AppendAllText` appends and creates the file if needed; `Directory.CreateDirectory` makes the folder, and does nothing if it's already there.
- In `Read`, count the lines as you go: the header is line 1. `decimal.TryParse` tells you whether the GPA is a number, and `Trim()` removes the spaces around a value.
- `new StreamWriter(path)` replaces a file; `new StreamWriter(path, append: true)` adds to it. Check `File.Exists` before you open the file, to know whether it needs a header.
- `{student.Gpa:0.00}` writes the GPA to two decimal places.
- In `ReadAll`, put the `try` around the one call to `StudentLine.Parse`, inside the loop, so the `catch` can still see `line` and the line number. `new FormatException(message, ex)` keeps `ex` as the `InnerException`; set `Data["LineNumber"]` before you `throw` it.
- `ImportHonorRoll` is two calls up from where the exception starts: one `try` around both calls is enough, and the exception skips `WriteHonorRoll` on its way to your `catch`.

## Hand it in

When it works, push your solution to a public repo of your own on GitHub, then paste that repo's address into **Submit your exercise** on the lesson page at [ask-codey.com](https://www.ask-codey.com). That's what completes the lesson. The lesson page shows the Git commands, step by step.
