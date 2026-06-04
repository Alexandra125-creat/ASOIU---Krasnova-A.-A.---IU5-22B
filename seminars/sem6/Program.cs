namespace Seminar6;

// ============================================================
// ЧАСТЬ 1. ПЛОХОЙ КОД (НАРУШАЕТ ВСЕ ПРИНЦИПЫ)
// ============================================================

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int YearsOfExperience { get; set; }
}

// Грязный код: один метод делает всё
public class EmployeeProcessorBad
{
    public void Do(List<Employee> e, string d, decimal m)
    {
        // Фильтрация
        List<Employee> r = new List<Employee>();
        for (int i = 0; i < e.Count; i++)
        {
            if (e[i].Department == d)
            {
                if (e[i].Salary >= m)
                {
                    if (!string.IsNullOrEmpty(e[i].Name))
                    {
                        r.Add(e[i]);
                    }
                }
            }
        }

        // Вывод сводки
        decimal t = 0;
        for (int j = 0; j < r.Count; j++)
        {
            t += r[j].Salary;
            Console.WriteLine("Name: " + r[j].Name + " Dept: " + r[j].Department + " Salary: " + r[j].Salary);
        }
        decimal avg = r.Count > 0 ? t / r.Count : 0;
        Console.WriteLine("Total: " + t + " Count: " + r.Count + " Avg: " + avg);

        // Экспорт в CSV
        string content = "";
        for (int k = 0; k < r.Count; k++)
            content += r[k].Name + "," + r[k].Department + "," + r[k].Salary + "\n";
        File.WriteAllText("report_" + d + "_" + DateTime.Now.Ticks + ".csv", content);
    }
}

// ============================================================
// ЧАСТЬ 2. РЕФАКТОРИНГ (ЧИСТЫЙ КОД)
// ============================================================

// Принцип: SRP + DIP
public interface IReportExporter
{
    void Export(IEnumerable<Employee> employees, string reportName);
}

public class CsvFileExporter : IReportExporter
{
    public void Export(IEnumerable<Employee> employees, string reportName)
    {
        string content = string.Join("\n", employees.Select(e => $"{e.Name},{e.Department},{e.Salary}"));
        File.WriteAllText($"{reportName}.csv", content);
    }
}

// Принцип: SRP - отдельная ответственность за фильтрацию
public class EmployeeFilter
{
    public List<Employee> FilterByDepartmentAndSalary(
        List<Employee> employees,
        string department,
        decimal minimumSalary)
    {
        return employees
            .Where(e => e.Department == department
                     && e.Salary >= minimumSalary
                     && !string.IsNullOrEmpty(e.Name))
            .ToList();
    }
}

// Принцип: SRP - отдельная ответственность за вывод на печать
public class EmployeePrinter
{
    public void PrintSummary(List<Employee> employees)
    {
        foreach (var employee in employees)
        {
            Console.WriteLine($"{employee.Name} | {employee.Department} | {employee.Salary:C}");
        }

        if (employees.Count == 0) return;

        decimal total = employees.Sum(e => e.Salary);
        decimal average = total / employees.Count;
        Console.WriteLine($"Total: {total:C} | Count: {employees.Count} | Avg: {average:C}");
    }
}

// Чистый класс: одна ответственность, зависимости через конструктор
public class EmployeeProcessorClean
{
    private readonly EmployeeFilter _filter;
    private readonly EmployeePrinter _printer;
    private readonly IReportExporter _exporter;

    public EmployeeProcessorClean(EmployeeFilter filter, EmployeePrinter printer, IReportExporter exporter)
    {
        _filter = filter;
        _printer = printer;
        _exporter = exporter;
    }

    public void ProcessDepartmentEmployees(List<Employee> employees, string department, decimal minimumSalary)
    {
        var filtered = _filter.FilterByDepartmentAndSalary(employees, department, minimumSalary);
        _printer.PrintSummary(filtered);
        _exporter.Export(filtered, $"report_{department}");
    }
}

// ============================================================
// ЧАСТЬ 3. ПРИМЕР НАРУШЕНИЯ SRP (КЛАСС СОТРУДНИКА)
// ============================================================

// ПЛОХО: класс делает всё сам
public class EmployeeBad
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int YearsOfExperience { get; set; }

    public bool IsValid() => !string.IsNullOrWhiteSpace(Name) && Salary > 0;

    public void SaveToDatabase(string connectionString)
    {
        // INSERT INTO Employees...
        Console.WriteLine($"Saving {Name} to database");
    }

    public string GeneratePayslip()
    {
        decimal tax = Salary * 0.13m;
        decimal netPay = Salary - tax;
        return $"Payslip\nEmployee: {Name}\nGross: {Salary:C}\nTax: {tax:C}\nNet: {netPay:C}";
    }
}

// ХОРОШО: каждая ответственность в отдельном классе
public class EmployeeGood
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int YearsOfExperience { get; set; }
}

public class EmployeeValidator
{
    public bool IsValid(EmployeeGood employee)
    {
        return !string.IsNullOrWhiteSpace(employee.Name) && employee.Salary > 0;
    }
}

public class EmployeeRepository
{
    private readonly string _connectionString;

    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(EmployeeGood employee)
    {
        Console.WriteLine($"Saving {employee.Name} to database with connection {_connectionString}");
    }
}

public class PayslipGenerator
{
    private const decimal IncomeTaxRate = 0.13m;

    public string Generate(EmployeeGood employee)
    {
        decimal tax = employee.Salary * IncomeTaxRate;
        decimal netPay = employee.Salary - tax;
        return $"Payslip\nEmployee: {employee.Name}\nDepartment: {employee.Department}\nGross: {employee.Salary:C}\nTax (13%): {tax:C}\nNet: {netPay:C}";
    }
}

// ============================================================
// ЧАСТЬ 4. ПРИНЦИП OCP (OPEN/CLOSED)
// ============================================================

// ПЛОХО: новый тип требует изменения существующего кода
public class LibraryItemReporterBad
{
    public void PrintInfo(object item)
    {
        if (item is BookBad book)
            Console.WriteLine($"[BOOK] {book.Title} - {book.Author}");
        else if (item is MagazineBad magazine)
            Console.WriteLine($"[MAGAZINE] {magazine.Title}, #{magazine.IssueNumber}");
        else if (item is DvdBad dvd)
            Console.WriteLine($"[DVD] {dvd.Title}, {dvd.DurationMinutes} min");
    }
}

public class BookBad { public string Title { get; set; } = ""; public string Author { get; set; } = ""; }
public class MagazineBad { public string Title { get; set; } = ""; public int IssueNumber { get; set; } }
public class DvdBad { public string Title { get; set; } = ""; public int DurationMinutes { get; set; } }

// ХОРОШО: открыт для расширения, закрыт для модификации
public abstract class LibraryItem
{
    public string Title { get; set; } = string.Empty;
    public abstract string GetDisplayInfo();
}

public class Book : LibraryItem
{
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public override string GetDisplayInfo() => $"[BOOK] {Title} - {Author} ({Year})";
}

public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }
    public override string GetDisplayInfo() => $"[MAGAZINE] {Title}, #{IssueNumber}";
}

public class Dvd : LibraryItem
{
    public int DurationMinutes { get; set; }
    public override string GetDisplayInfo() => $"[DVD] {Title}, {DurationMinutes} min";
}

public class LibraryItemReporterGood
{
    public void PrintInfo(LibraryItem item) => Console.WriteLine(item.GetDisplayInfo());
}

// ============================================================
// ЧАСТЬ 5. ПРИНЦИП LSP (LISKOV SUBSTITUTION)
// ============================================================

// ПЛОХО: квадрат не может заменить прямоугольник
public class RectangleBad
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }
    public int Area() => Width * Height;
}

public class SquareBad : RectangleBad
{
    public override int Width
    {
        get => base.Width;
        set { base.Width = value; base.Height = value; }
    }

    public override int Height
    {
        get => base.Height;
        set { base.Width = value; base.Height = value; }
    }
}

// ХОРОШО: общий предок без лишних обязательств
public abstract class Shape
{
    public abstract int Area();
}

public class Rectangle : Shape
{
    public int Width { get; set; }
    public int Height { get; set; }
    public override int Area() => Width * Height;
}

public class Square : Shape
{
    public int Side { get; set; }
    public override int Area() => Side * Side;
}

// ============================================================
// ЧАСТЬ 6. ПРИНЦИП ISP (INTERFACE SEGREGATION)
// ============================================================

// ПЛОХО: толстый интерфейс
public interface IWorkerBad
{
    void Work();
    void Eat();
    void Sleep();
}

public class HumanWorkerBad : IWorkerBad
{
    public void Work() => Console.WriteLine("Working");
    public void Eat() => Console.WriteLine("Eating");
    public void Sleep() => Console.WriteLine("Sleeping");
}

public class RobotWorkerBad : IWorkerBad
{
    public void Work() => Console.WriteLine("Working");
    public void Eat() => throw new NotSupportedException("Robots don't eat");
    public void Sleep() => throw new NotSupportedException("Robots don't sleep");
}

// ХОРОШО: разделённые интерфейсы
public interface IWorkable { void Work(); }
public interface IFeedable { void Eat(); }
public interface ISleepable { void Sleep(); }

public class HumanWorker : IWorkable, IFeedable, ISleepable
{
    public void Work() => Console.WriteLine("Working");
    public void Eat() => Console.WriteLine("Eating");
    public void Sleep() => Console.WriteLine("Sleeping");
}

public class RobotWorker : IWorkable
{
    public void Work() => Console.WriteLine("Working");
}

// ============================================================
// ЧАСТЬ 7. ПРИНЦИП DIP (DEPENDENCY INVERSION)
// ============================================================

// ПЛОХО: бизнес-логика зависит от конкретных реализаций
public class LibraryServiceBad
{
    private readonly string _connectionString = "Data Source=library.db";

    public void AddBook(string title, string author)
    {
        Console.WriteLine($"Saving book {title} to SQLite");
        Console.WriteLine($"Logging: Book {title} added");
    }
}

// ХОРОШО: зависимость от абстракций
public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

public interface IBookRepository
{
    void Save(string title, string author);
}

public class SqliteBookRepository : IBookRepository
{
    private readonly string _connectionString;

    public SqliteBookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Save(string title, string author)
    {
        Console.WriteLine($"Saving {title} to SQLite ({_connectionString})");
    }
}

public class LibraryServiceGood
{
    private readonly IBookRepository _repository;
    private readonly ILogger _logger;

    public LibraryServiceGood(IBookRepository repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public void AddBook(string title, string author)
    {
        _repository.Save(title, author);
        _logger.Log($"Book added: {title}");
    }
}

// ============================================================
// ЧАСТЬ 8. ПРАКТИЧЕСКОЕ ЗАДАНИЕ (ИСХОДНЫЙ КОД С НАРУШЕНИЯМИ)
// ============================================================

public class LibraryServiceForAssignment
{
    private List<object> _items = new List<object>();

    public void AddBook(string title, string author, int year)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название не может быть пустым");
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор не может быть пустым");
        if (year < 1000 || year > DateTime.Now.Year)
            throw new ArgumentException("Некорректный год издания");

        _items.Add(new { Type = "Book", Title = title, Author = author, Year = year });
        File.AppendAllText("library.log", $"{DateTime.Now}: Добавлена книга \"{title}\"\n");
    }

    public void AddMagazine(string title, int issueNumber)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название не может быть пустым");
        if (issueNumber <= 0)
            throw new ArgumentException("Номер выпуска должен быть положительным");

        _items.Add(new { Type = "Magazine", Title = title, IssueNumber = issueNumber });
        File.AppendAllText("library.log", $"{DateTime.Now}: Добавлен журнал \"{title}\"\n");
    }

    public void PrintReport()
    {
        Console.WriteLine($"=== Отчёт: {_items.Count} элементов ===");
        foreach (var item in _items)
        {
            string title = item.GetType().GetProperty("Title")?.GetValue(item)?.ToString() ?? "Без названия";
            bool isBook = item.GetType().GetProperty("Author") != null;
            Console.WriteLine(isBook ? $"Книга: {title}" : $"Журнал: {title}");
        }
        File.WriteAllText("report.txt", $"Всего элементов: {_items.Count}\nДата: {DateTime.Now}");
    }
}

// ============================================================
// ЧАСТЬ 9. ИСПРАВЛЕННЫЙ КОД (ПОСЛЕ РЕФАКТОРИНГА)
// ============================================================

// Интерфейсы для DIP
public interface ILibraryLogger
{
    void Log(string message);
}

public class FileLibraryLogger : ILibraryLogger
{
    private readonly string _filePath;

    public FileLibraryLogger(string filePath = "library.log")
    {
        _filePath = filePath;
    }

    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"{DateTime.Now}: {message}\n");
    }
}

// Абстракция для LibraryItem (OCP + LSP)
public abstract class LibraryItemBase
{
    public string Title { get; set; } = string.Empty;
    public abstract string GetDisplayInfo();
}

public class LibraryBook : LibraryItemBase
{
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }

    public override string GetDisplayInfo() => $"Книга: {Title}";
}

public class LibraryMagazine : LibraryItemBase
{
    public int IssueNumber { get; set; }

    public override string GetDisplayInfo() => $"Журнал: {Title}";
}

// Валидация - отдельная ответственность (SRP + DRY)
public class LibraryItemValidator
{
    public void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Название не может быть пустым");
    }

    public void ValidateBook(string title, string author, int year)
    {
        ValidateTitle(title);
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор не может быть пустым");
        if (year < 1000 || year > DateTime.Now.Year)
            throw new ArgumentException("Некорректный год издания");
    }

    public void ValidateMagazine(string title, int issueNumber)
    {
        ValidateTitle(title);
        if (issueNumber <= 0)
            throw new ArgumentException("Номер выпуска должен быть положительным");
    }
}

// Отчёт - отдельная ответственность (SRP)
public class LibraryReportPrinter
{
    public void PrintReport(IEnumerable<LibraryItemBase> items)
    {
        Console.WriteLine($"=== Отчёт: {items.Count()} элементов ===");
        foreach (var item in items)
        {
            Console.WriteLine(item.GetDisplayInfo());
        }
    }

    public void SaveReportToFile(IEnumerable<LibraryItemBase> items, string filePath)
    {
        string content = $"Всего элементов: {items.Count()}\nДата: {DateTime.Now}\n";
        content += string.Join("\n", items.Select(i => i.GetDisplayInfo()));
        File.WriteAllText(filePath, content);
    }
}

// Основной сервис (зависит только от абстракций)
public class LibraryServiceRefactored
{
    private readonly List<LibraryItemBase> _items = new();
    private readonly LibraryItemValidator _validator;
    private readonly ILibraryLogger _logger;
    private readonly LibraryReportPrinter _reportPrinter;

    public LibraryServiceRefactored(LibraryItemValidator validator, ILibraryLogger logger, LibraryReportPrinter reportPrinter)
    {
        _validator = validator;
        _logger = logger;
        _reportPrinter = reportPrinter;
    }

    public void AddBook(string title, string author, int year)
    {
        _validator.ValidateBook(title, author, year);
        _items.Add(new LibraryBook { Title = title, Author = author, Year = year });
        _logger.Log($"Добавлена книга \"{title}\"");
    }

    public void AddMagazine(string title, int issueNumber)
    {
        _validator.ValidateMagazine(title, issueNumber);
        _items.Add(new LibraryMagazine { Title = title, IssueNumber = issueNumber });
        _logger.Log($"Добавлен журнал \"{title}\"");
    }

    public void PrintReport()
    {
        _reportPrinter.PrintReport(_items);
    }

    public void SaveReportToFile(string filePath = "report.txt")
    {
        _reportPrinter.SaveReportToFile(_items, filePath);
    }
}

// ============================================================
// ГЛАВНАЯ ПРОГРАММА (ТОЧКА СБОРКИ)
// ============================================================

class Program
{
    static void Main()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("SEMINAR 6. REFACTORING AND SOLID PRINCIPLES");
        Console.WriteLine("==============================================\n");

        // Часть 1. Демонстрация грязного кода
        Console.WriteLine("1. DIRTY CODE (BEFORE REFACTORING)");
        Console.WriteLine("----------------------------------------------\n");

        var employees = new List<Employee>
        {
            new() { Name = "Ivan", Department = "IT", Salary = 100000, YearsOfExperience = 6 },
            new() { Name = "Petr", Department = "IT", Salary = 80000, YearsOfExperience = 3 },
            new() { Name = "", Department = "IT", Salary = 50000, YearsOfExperience = 1 },
        };

        var badProcessor = new EmployeeProcessorBad();
        badProcessor.Do(employees, "IT", 60000);

        Console.WriteLine("\n");

        // Часть 2. Чистый код после рефакторинга
        Console.WriteLine("2. CLEAN CODE (AFTER REFACTORING)");
        Console.WriteLine("----------------------------------------------\n");

        var filter = new EmployeeFilter();
        var printer = new EmployeePrinter();
        var exporter = new CsvFileExporter();
        var cleanProcessor = new EmployeeProcessorClean(filter, printer, exporter);

        cleanProcessor.ProcessDepartmentEmployees(employees, "IT", 60000);

        Console.WriteLine("\n");

        // Часть 3. Демонстрация SOLID
        Console.WriteLine("3. SOLID PRINCIPLES DEMONSTRATION");
        Console.WriteLine("----------------------------------------------\n");

        // SRP
        Console.WriteLine("  SRP - Single Responsibility Principle:");
        var employee = new EmployeeGood { Name = "Alex", Department = "HR", Salary = 90000 };
        var validator = new EmployeeValidator();
        var repo = new EmployeeRepository("Server=localhost");
        var payslipGen = new PayslipGenerator();

        Console.WriteLine($"    Valid: {validator.IsValid(employee)}");
        repo.Save(employee);
        Console.WriteLine(payslipGen.Generate(employee));

        Console.WriteLine();

        // OCP
        Console.WriteLine("  OCP - Open/Closed Principle:");
        var reporter = new LibraryItemReporterGood();
        var book = new Book { Title = "War and Peace", Author = "Tolstoy", Year = 1869 };
        var magazine = new Magazine { Title = "Science", IssueNumber = 42 };
        var dvd = new Dvd { Title = "Inception", DurationMinutes = 148 };

        reporter.PrintInfo(book);
        reporter.PrintInfo(magazine);
        reporter.PrintInfo(dvd);

        Console.WriteLine();

        // LSP
        Console.WriteLine("  LSP - Liskov Substitution Principle:");
        Shape rect = new Rectangle { Width = 5, Height = 10 };
        Shape square = new Square { Side = 5 };
        Console.WriteLine($"    Rectangle area: {rect.Area()}");
        Console.WriteLine($"    Square area: {square.Area()}");

        Console.WriteLine();

        // ISP
        Console.WriteLine("  ISP - Interface Segregation Principle:");
        var human = new HumanWorker();
        var robot = new RobotWorker();
        human.Work(); human.Eat(); human.Sleep();
        robot.Work();

        Console.WriteLine();

        // DIP
        Console.WriteLine("  DIP - Dependency Inversion Principle:");
        IBookRepository sqlRepo = new SqliteBookRepository("Data Source=library.db");
        ILogger logger = new ConsoleLogger();
        var libService = new LibraryServiceGood(sqlRepo, logger);
        libService.AddBook("Clean Code", "Robert Martin");

        Console.WriteLine("\n");

        // Часть 4. Практическое задание
        Console.WriteLine("4. PRACTICAL ASSIGNMENT (REFACTORING)");
        Console.WriteLine("----------------------------------------------\n");

        Console.WriteLine("  BEFORE REFACTORING (with violations):");
        var badLibrary = new LibraryServiceForAssignment();
        badLibrary.AddBook("Test Book", "Test Author", 2024);
        badLibrary.AddMagazine("Test Magazine", 5);
        badLibrary.PrintReport();

        Console.WriteLine("\n  AFTER REFACTORING (fixed):");
        var validatorLib = new LibraryItemValidator();
        var fileLogger = new FileLibraryLogger();
        var reportPrinter = new LibraryReportPrinter();
        var goodLibrary = new LibraryServiceRefactored(validatorLib, fileLogger, reportPrinter);

        goodLibrary.AddBook("Clean Architecture", "Robert Martin", 2024);
        goodLibrary.AddMagazine("Tech Review", 10);
        goodLibrary.PrintReport();
        goodLibrary.SaveReportToFile("refactored_report.txt");

        Console.WriteLine("\n  DONE! Check 'refactored_report.txt' file.");

        Console.WriteLine("\nProgram completed successfully!");
    }
}