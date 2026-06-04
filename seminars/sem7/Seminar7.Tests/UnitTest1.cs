using Moq;
using Shouldly;
using Seminar7;

namespace Seminar7.Tests;

public class RegularFineRuleTests
{
    [Fact]
    public void Calculate_ThreeDaysOverdue_Returns15()
    {
        var rule = new RegularFineRule();
        decimal fine = rule.Calculate(3);
        Assert.Equal(15m, fine);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 5)]
    [InlineData(3, 15)]
    [InlineData(10, 50)]
    public void Calculate_VariousDays_ReturnsCorrectFine(int days, decimal expected)
    {
        var rule = new RegularFineRule();
        decimal fine = rule.Calculate(days);
        Assert.Equal(expected, fine);
    }
}

public class FamilyFineRuleTests
{
    [Fact]
    public void Calculate_WithinGracePeriod_ReturnsZero()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(3);
        Assert.Equal(0m, fine);
    }

    [Fact]
    public void Calculate_AfterGracePeriod_Charges2RublesPerDay()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(8);
        Assert.Equal(6m, fine);
    }

    [Fact]
    public void Calculate_FineIsCappedAt100Rubles()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(100);
        Assert.Equal(100m, fine);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(5, 0)]
    [InlineData(6, 2)]
    [InlineData(10, 10)]
    [InlineData(55, 100)]
    [InlineData(100, 100)]
    public void Calculate_TableOfScenarios_ReturnsExpectedFine(int days, decimal expected)
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(days);
        Assert.Equal(expected, fine);
    }
}

public class StubEmployeeRepository : IEmployeeRepository
{
    private readonly Employee? _employee;
    public StubEmployeeRepository(Employee? employee) => _employee = employee;
    public Employee? GetById(int id) => _employee;
}

public class StubClock : IClock
{
    private readonly DateTime _fixedTime;
    public StubClock(DateTime fixedTime) => _fixedTime = fixedTime;
    public DateTime Now => _fixedTime;
}

public class NullLogger : ILogger
{
    public void Log(string message) { }
}

public class BonusCalculatorManualStubsTests
{
    [Fact]
    public void Calculate_NotDecember_ReturnsZero()
    {
        var employee = new Employee
        {
            Id = 1,
            Name = "Ivanov",
            Salary = 100_000m,
            YearsOfExperience = 7,
            IsFullTime = true
        };
        
        var repository = new StubEmployeeRepository(employee);
        var clock = new StubClock(new DateTime(2025, 6, 15));
        var logger = new NullLogger();
        
        var calculator = new BonusCalculator(repository, clock, logger);
        decimal bonus = calculator.Calculate(1);
        
        Assert.Equal(0m, bonus);
    }

    [Fact]
    public void Calculate_SeniorFullTimeInDecember_Returns15Percent()
    {
        var employee = new Employee
        {
            Id = 1,
            Name = "Ivanov",
            Salary = 100_000m,
            YearsOfExperience = 7,
            IsFullTime = true
        };
        
        var repository = new StubEmployeeRepository(employee);
        var clock = new StubClock(new DateTime(2025, 12, 15));
        var logger = new NullLogger();
        
        var calculator = new BonusCalculator(repository, clock, logger);
        decimal bonus = calculator.Calculate(1);
        
        Assert.Equal(15_000m, bonus);
    }
}

public class BonusCalculatorMoqTests
{
    [Fact]
    public void Calculate_FullTimeSeniorInDecember_Returns15PercentAndLogsEvent()
    {
        var employee = new Employee
        {
            Id = 1,
            Name = "Ivanov",
            Salary = 100_000m,
            YearsOfExperience = 7,
            IsFullTime = true
        };
        
        var repositoryMock = new Mock<IEmployeeRepository>();
        var clockMock = new Mock<IClock>();
        var loggerMock = new Mock<ILogger>();
        
        repositoryMock.Setup(r => r.GetById(1)).Returns(employee);
        clockMock.Setup(c => c.Now).Returns(new DateTime(2025, 12, 15));
        
        var calculator = new BonusCalculator(
            repositoryMock.Object,
            clockMock.Object,
            loggerMock.Object);
        
        decimal bonus = calculator.Calculate(1);
        
        Assert.Equal(15_000m, bonus);
        
        loggerMock.Verify(
            l => l.Log(It.Is<string>(s => s.Contains("Ivanov"))),
            Times.Once);
    }

    [Fact]
    public void Calculate_EmployeeNotFound_ThrowsArgumentException()
    {
        var repositoryMock = new Mock<IEmployeeRepository>();
        var clockMock = new Mock<IClock>();
        var loggerMock = new Mock<ILogger>();
        
        repositoryMock.Setup(r => r.GetById(99)).Returns((Employee?)null);
        
        var calculator = new BonusCalculator(
            repositoryMock.Object,
            clockMock.Object,
            loggerMock.Object);
        
        var exception = Assert.Throws<ArgumentException>(
            () => calculator.Calculate(99));
        
        Assert.Contains("99", exception.Message);
    }

    [Fact]
    public void Calculate_PartTimeEmployee_Returns7Percent()
    {
        var employee = new Employee
        {
            Id = 1,
            Name = "Petrov",
            Salary = 50_000m,
            YearsOfExperience = 3,
            IsFullTime = false
        };
        
        var repositoryMock = new Mock<IEmployeeRepository>();
        var clockMock = new Mock<IClock>();
        var loggerMock = new Mock<ILogger>();
        
        repositoryMock.Setup(r => r.GetById(1)).Returns(employee);
        clockMock.Setup(c => c.Now).Returns(new DateTime(2025, 12, 15));
        
        var calculator = new BonusCalculator(
            repositoryMock.Object,
            clockMock.Object,
            loggerMock.Object);
        
        decimal bonus = calculator.Calculate(1);
        
        Assert.Equal(3_500m, bonus);
    }
}

public class FamilyFineRuleShouldlyTests
{
    [Fact]
    public void Calculate_WithinGracePeriod_ShouldReturnZero()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(3);
        fine.ShouldBe(0m);
    }

    [Fact]
    public void Calculate_AfterGracePeriod_ShouldCharge2RublesPerDay()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(8);
        fine.ShouldBe(6m);
    }

    [Fact]
    public void Calculate_MaximumFine_ShouldNotExceed100()
    {
        var rule = new FamilyFineRule();
        decimal fine = rule.Calculate(100);
        fine.ShouldBeLessThanOrEqualTo(100m);
        fine.ShouldBe(100m);
    }
}
