namespace Seminar7;

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[LOG] {message}");
}

public interface IClock
{
    DateTime Now { get; }
}

public class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
}

public interface IEmployeeRepository
{
    Employee? GetById(int id);
}

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public int YearsOfExperience { get; set; }
    public bool IsFullTime { get; set; }
}

public class RegularFineRule
{
    public decimal Calculate(int daysOverdue) => daysOverdue * 5m;
}

public class FamilyFineRule
{
    private const int GracePeriodDays = 5;
    private const decimal RateAfterGrace = 2m;
    private const decimal MaximumFine = 100m;

    public decimal Calculate(int daysOverdue)
    {
        if (daysOverdue <= 0) return 0m;
        
        if (daysOverdue <= GracePeriodDays)
            return 0m;
        
        decimal fine = (daysOverdue - GracePeriodDays) * RateAfterGrace;
        
        return Math.Min(fine, MaximumFine);
    }
}

public class BonusCalculator
{
    private readonly IEmployeeRepository _repository;
    private readonly IClock _clock;
    private readonly ILogger _logger;

    public BonusCalculator(IEmployeeRepository repository, IClock clock, ILogger logger)
    {
        _repository = repository;
        _clock = clock;
        _logger = logger;
    }

    public decimal Calculate(int employeeId)
    {
        var employee = _repository.GetById(employeeId);
        
        if (employee == null)
            throw new ArgumentException($"Employee {employeeId} not found");

        if (_clock.Now.Month != 12)
            return 0m;

        _logger.Log($"Calculating bonus for {employee.Name}");

        if (!employee.IsFullTime)
            return employee.Salary * 0.07m;

        if (employee.YearsOfExperience > 5)
            return employee.Salary * 0.15m;

        return employee.Salary * 0.12m;
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Seminar 7 - Testing in .NET");
        Console.WriteLine("Run 'dotnet test' in the test project to run unit tests");
    }
}
