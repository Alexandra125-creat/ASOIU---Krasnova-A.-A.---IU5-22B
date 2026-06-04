using Microsoft.Extensions.DependencyInjection;

namespace Seminar5;

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[CONSOLE LOG] {message}");
    }
}

public class FileLogger : ILogger
{
    private readonly string _filePath;
    
    public FileLogger(string filePath = "log.txt")
    {
        _filePath = filePath;
    }
    
    public void Log(string message)
    {
        File.AppendAllText(_filePath, $"[FILE LOG] {message}\n");
    }
}

public class NullLogger : ILogger
{
    public void Log(string message) { }
}

public interface IBookStorage
{
    void Save(string title, string author);
}

public class InMemoryBookStorage : IBookStorage
{
    private readonly ILogger _logger;
    private readonly List<string> _books = new();
    
    public InMemoryBookStorage(ILogger logger)
    {
        _logger = logger;
    }
    
    public void Save(string title, string author)
    {
        _books.Add($"\"{title}\" - {author}");
        _logger.Log($"[STORAGE] Saved: \"{title}\"");
    }
}

public class NaiveConsoleLogger
{
    public void Log(string message)
    {
        Console.WriteLine($"[LOG] {message}");
    }
}

public class NaiveBookCatalogService
{
    private NaiveConsoleLogger _logger = new NaiveConsoleLogger();
    
    public void AddBook(string title, string author)
    {
        _logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public class BookCatalogService_Constructor
{
    private readonly ILogger _logger;
    
    public BookCatalogService_Constructor(ILogger logger)
    {
        _logger = logger;
    }
    
    public void AddBook(string title, string author)
    {
        _logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public class BookCatalogService_Property
{
    public ILogger Logger { get; set; } = new NullLogger();
    
    public void AddBook(string title, string author)
    {
        Logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public class BookCatalogService_Method
{
    public void AddBook(string title, string author, ILogger logger)
    {
        logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public class BookCatalogService
{
    private readonly ILogger _logger;
    private readonly IBookStorage _storage;
    
    public BookCatalogService(ILogger logger, IBookStorage storage)
    {
        _logger = logger;
        _storage = storage;
    }
    
    public void AddBook(string title, string author)
    {
        _storage.Save(title, author);
        _logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public static class ServiceLocator
{
    private static ServiceProvider? _provider;
    
    public static void Init(ServiceProvider provider)
    {
        _provider = provider;
    }
    
    public static T Get<T>() where T : notnull
    {
        return _provider!.GetRequiredService<T>();
    }
}

public static class LoggerContext
{
    public static ILogger Current { get; set; } = new NullLogger();
}

public class BookCatalogService_Bastard
{
    private readonly ILogger _logger;
    
    public BookCatalogService_Bastard() : this(new ConsoleLogger()) { }
    
    public BookCatalogService_Bastard(ILogger logger)
    {
        _logger = logger;
    }
    
    public void AddBook(string title, string author)
    {
        _logger.Log($"Added book: \"{title}\" - {author}");
    }
}

public class BookCatalogService_AntiPatterns
{
    private IBookStorage _storage;
    
    public BookCatalogService_AntiPatterns()
        : this(new InMemoryBookStorage(new ConsoleLogger())) { }
    
    public BookCatalogService_AntiPatterns(IBookStorage storage)
    {
        _storage = storage;
    }
    
    public void AddBook(string title, string author)
    {
        ILogger logger = ServiceLocator.Get<ILogger>();
        _storage.Save(title, author);
        logger.Log($"Added book: \"{title}\" - {author}");
    }
    
    public void RemoveBook(string title)
    {
        LoggerContext.Current.Log($"Removed book: \"{title}\"");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("SEMINAR 5. DEPENDENCY INJECTION");
        Console.WriteLine("==============================================\n");
        
        Console.WriteLine("1. STRONGLY COUPLED CODE (BAD)");
        Console.WriteLine("----------------------------------------------\n");
        
        var badService = new NaiveBookCatalogService();
        badService.AddBook("Eugene Onegin", "Pushkin");
        Console.WriteLine("  Problem: cannot change logger without modifying code\n");
        
        Console.WriteLine("2. CONSTRUCTOR INJECTION");
        Console.WriteLine("----------------------------------------------\n");
        
        var service1 = new BookCatalogService_Constructor(new ConsoleLogger());
        service1.AddBook("War and Peace", "Tolstoy");
        
        var service2 = new BookCatalogService_Constructor(new FileLogger("constructor_log.txt"));
        service2.AddBook("Crime and Punishment", "Dostoevsky");
        
        Console.WriteLine();
        
        Console.WriteLine("3. PROPERTY INJECTION (OPTIONAL)");
        Console.WriteLine("----------------------------------------------\n");
        
        var service3 = new BookCatalogService_Property();
        service3.AddBook("Without logger (NullLogger)", "Test");
        
        service3.Logger = new ConsoleLogger();
        service3.AddBook("With connected logger", "Test");
        
        Console.WriteLine();
        
        Console.WriteLine("4. METHOD INJECTION");
        Console.WriteLine("----------------------------------------------\n");
        
        var service4 = new BookCatalogService_Method();
        service4.AddBook("Method log", "Example", new ConsoleLogger());
        service4.AddBook("Method log to file", "Example", new FileLogger("method_log.txt"));
        
        Console.WriteLine();
        
        Console.WriteLine("5. COMPOSITION ROOT (PURE DI)");
        Console.WriteLine("----------------------------------------------\n");
        
        ILogger logger = new ConsoleLogger();
        IBookStorage storage = new InMemoryBookStorage(logger);
        var service5 = new BookCatalogService(logger, storage);
        
        service5.AddBook("The Captain's Daughter", "Pushkin");
        service5.AddBook("Dead Souls", "Gogol");
        
        Console.WriteLine();
        
        Console.WriteLine("6. DI CONTAINER (Microsoft.Extensions.DependencyInjection)");
        Console.WriteLine("----------------------------------------------\n");
        
        var services = new ServiceCollection();
        
        services.AddSingleton<ILogger, ConsoleLogger>();
        services.AddSingleton<IBookStorage, InMemoryBookStorage>();
        services.AddTransient<BookCatalogService>();
        
        var provider = services.BuildServiceProvider(
            new ServiceProviderOptions { ValidateOnBuild = true }
        );
        
        var service6 = provider.GetRequiredService<BookCatalogService>();
        service6.AddBook("DI Container", "Demonstration");
        
        Console.WriteLine("\n  Lifetime demonstration:");
        var logger1 = provider.GetRequiredService<ILogger>();
        var logger2 = provider.GetRequiredService<ILogger>();
        Console.WriteLine($"  Singleton ILogger: same objects? {ReferenceEquals(logger1, logger2)}");
        
        var service6a = provider.GetRequiredService<BookCatalogService>();
        var service6b = provider.GetRequiredService<BookCatalogService>();
        Console.WriteLine($"  Transient BookCatalogService: different objects? {!ReferenceEquals(service6a, service6b)}");
        
        Console.WriteLine();
        
        Console.WriteLine("7. DEPENDENCY INJECTION ANTIPATTERNS");
        Console.WriteLine("----------------------------------------------\n");
        
        Console.WriteLine("  ANTIPATTERN: Service Locator");
        ServiceLocator.Init(provider);
        var loggerFromLocator = ServiceLocator.Get<ILogger>();
        loggerFromLocator.Log("Message via Service Locator");
        Console.WriteLine("     Problem: dependencies hidden from API!\n");
        
        Console.WriteLine("  ANTIPATTERN: Ambient Context");
        LoggerContext.Current.Log("Message via Ambient Context");
        Console.WriteLine("     Problem: global state and hidden dependencies!\n");
        
        Console.WriteLine("  ANTIPATTERN: Bastard Injection");
        var bastardService = new BookCatalogService_Bastard();
        bastardService.AddBook("Bastard Test", "Test");
        Console.WriteLine("     Problem: parameterless constructor creates illusion of independence!\n");
        
        Console.WriteLine("  CONTROL TASK (all antipatterns together)");
        var badAllService = new BookCatalogService_AntiPatterns();
        badAllService.AddBook("Antipattern Book", "Author");
        badAllService.RemoveBook("Antipattern Book");
        
        Console.WriteLine("\n  FIX: remove default constructor, pass dependencies via constructor, remove ServiceLocator and Ambient Context\n");
        
        Console.WriteLine("8. SCOPED LIFETIME");
        Console.WriteLine("----------------------------------------------\n");
        
        var scopedServices = new ServiceCollection();
        scopedServices.AddScoped<IBookStorage, InMemoryBookStorage>();
        var scopedProvider = scopedServices.BuildServiceProvider();
        
        using (var scope1 = scopedProvider.CreateScope())
        {
            var storage1 = scope1.ServiceProvider.GetRequiredService<IBookStorage>();
            var storage2 = scope1.ServiceProvider.GetRequiredService<IBookStorage>();
            Console.WriteLine($"  Scoped: same objects in one scope? {ReferenceEquals(storage1, storage2)}");
        }
        
        using (var scope2 = scopedProvider.CreateScope())
        {
            var storage3 = scope2.ServiceProvider.GetRequiredService<IBookStorage>();
            var storage4 = scope2.ServiceProvider.GetRequiredService<IBookStorage>();
            Console.WriteLine($"  Scoped: same objects in another scope? {ReferenceEquals(storage3, storage4)}");
        }
        
        Console.WriteLine("\nProgram completed successfully!");
    }
}
