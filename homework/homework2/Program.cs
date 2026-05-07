using Homework2;
using System.Text;

//Код взят из методических указаний и адаптирован под мой вариант
// Устанавливаем кодировку для корректного отображения русского языка
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// Пути к файлам
string dbPath = "publishers_journals.db";
string publishersCsv = Path.Combine(AppContext.BaseDirectory, "publishers.csv");
string journalsCsv = Path.Combine(AppContext.BaseDirectory, "journals.csv");

// Создаём менеджер БД и импортируем данные из CSV
var db = new DatabaseManager(dbPath);
db.ImportFromCsv(publishersCsv, journalsCsv);

Console.WriteLine();
Console.WriteLine("╔════════════════════════════════════════╗");
Console.WriteLine("║   БАЗА ДАННЫХ ИЗДАТЕЛЬСТВ И ЖУРНАЛОВ   ║");
Console.WriteLine("╚════════════════════════════════════════╝");
Console.WriteLine();

// Главный цикл меню
string choice;
do
{
    Console.WriteLine("╔════════════════════════════════════════╗");
    Console.WriteLine("║            ГЛАВНОЕ МЕНЮ                ║");
    Console.WriteLine("╠════════════════════════════════════════╣");
    Console.WriteLine("║ 1 — Показать все издательства          ║");
    Console.WriteLine("║ 2 — Показать все журналы               ║");
    Console.WriteLine("║ 3 — Добавить журнал                    ║");
    Console.WriteLine("║ 4 — Редактировать журнал               ║");
    Console.WriteLine("║ 5 — Удалить журнал                     ║");
    Console.WriteLine("║ 6 — Отчёты                             ║");
    Console.WriteLine("║ 0 — Выход                              ║");
    Console.WriteLine("╚════════════════════════════════════════╝");
    Console.Write("Ваш выбор: ");

    choice = Console.ReadLine()?.Trim() ?? "";
    Console.WriteLine();

    switch (choice)
    {
        case "1": ShowPublishers(db); break;
        case "2": ShowJournals(db); break;
        case "3": AddJournal(db); break;
        case "4": EditJournal(db); break;
        case "5": DeleteJournal(db); break;
        case "6": ReportsMenu(db); break;
        case "0": Console.WriteLine("До свидания!"); break;
        default: Console.WriteLine("Неверный пункт меню."); break;
    }
    Console.WriteLine();
} while (choice != "0");

// ═══════════════════════════════════════════════════════════════════
// Функции для работы с данными
// ═══════════════════════════════════════════════════════════════════

static void ShowPublishers(DatabaseManager db)
{
    Console.WriteLine("═══════════════ ВСЕ ИЗДАТЕЛЬСТВА ═══════════════");
    var publishers = db.GetAllPublishers();
    foreach (var pub in publishers)
        Console.WriteLine("  " + pub);
    Console.WriteLine($"────────────────────────────────────────────────");
    Console.WriteLine($"Итого издательств: {publishers.Count}");
}

static void ShowJournals(DatabaseManager db)
{
    Console.WriteLine("═════════════════ ВСЕ ЖУРНАЛЫ ═════════════════");
    var journals = db.GetAllJournals();
    foreach (var jrn in journals)
        Console.WriteLine("  " + jrn);
    Console.WriteLine($"────────────────────────────────────────────────");
    Console.WriteLine($"Итого журналов: {journals.Count}");
}

static void AddJournal(DatabaseManager db)
{
    Console.WriteLine("═══════════════ ДОБАВЛЕНИЕ ЖУРНАЛА ═══════════════");

    // Показываем доступные издательства
    Console.WriteLine("Доступные издательства:");
    var publishers = db.GetAllPublishers();
    foreach (var pub in publishers)
        Console.WriteLine("  " + pub);
    Console.WriteLine();

    // Ввод ID издательства
    Console.Write("Введите ID издательства: ");
    if (!int.TryParse(Console.ReadLine(), out int publisherId))
    {
        Console.WriteLine(" Ошибка: введите целое число.");
        return;
    }

    // Проверяем, что издательство существует
    bool publisherExists = publishers.Any(p => p.Id == publisherId);
    if (!publisherExists)
    {
        Console.WriteLine("Ошибка: издательство с таким ID не найдено.");
        return;
    }

    // Ввод названия журнала
    Console.Write("Введите название журнала: ");
    string name = Console.ReadLine()?.Trim() ?? "";
    if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Ошибка: название не может быть пустым.");
        return;
    }

    // Ввод тиража
    Console.Write("Введите тираж (тыс. экземпляров): ");
    if (!int.TryParse(Console.ReadLine(), out int circulation))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    try
    {
        var journal = new Journal(0, publisherId, name, circulation);
        db.AddJournal(journal);
        Console.WriteLine("Журнал успешно добавлен!");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($" Ошибка: {ex.Message}");
    }
}

static void EditJournal(DatabaseManager db)
{
    Console.WriteLine("═══════════════ РЕДАКТИРОВАНИЕ ЖУРНАЛА ═══════════════");

    Console.Write("Введите ID журнала для редактирования: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    var journal = db.GetJournalById(id);
    if (journal == null)
    {
        Console.WriteLine($"Ошибка: журнал с ID={id} не найден.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"Текущие данные: {journal}");
    Console.WriteLine();
    Console.WriteLine("(Нажмите Enter, чтобы оставить значение без изменений)");
    Console.WriteLine();

    // Редактирование названия
    Console.Write($"Название журнала [{journal.Name}]: ");
    string input = Console.ReadLine()?.Trim() ?? "";
    if (!string.IsNullOrEmpty(input))
        journal.Name = input;

    // Редактирование ID издательства
    Console.Write($"ID издательства [{journal.PublisherId}]: ");
    input = Console.ReadLine()?.Trim() ?? "";
    if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int newPublisherId))
    {
        // Проверяем существование издательства
        var publishers = db.GetAllPublishers();
        if (publishers.Any(p => p.Id == newPublisherId))
            journal.PublisherId = newPublisherId;
        else
            Console.WriteLine(" Предупреждение: издательство с таким ID не найдено, ID не изменён.");
    }

    // Редактирование тиража
    Console.Write($"Тираж (тыс. экз.) [{journal.CirculationK}]: ");
    input = Console.ReadLine()?.Trim() ?? "";
    if (!string.IsNullOrEmpty(input) && int.TryParse(input, out int newCirculation))
    {
        try
        {
            journal.CirculationK = newCirculation;
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return;
        }
    }

    db.UpdateJournal(journal);
    Console.WriteLine("Данные журнала обновлены!");
}

static void DeleteJournal(DatabaseManager db)
{
    Console.WriteLine("════════════════ УДАЛЕНИЕ ЖУРНАЛА ════════════════");

    Console.Write("Введите ID журнала для удаления: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        return;
    }

    var journal = db.GetJournalById(id);
    if (journal == null)
    {
        Console.WriteLine($"Ошибка: журнал с ID={id} не найден.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine($"Вы действительно хотите удалить журнал?");
    Console.WriteLine($"  {journal}");
    Console.WriteLine();
    Console.Write("Введите 'YES' для подтверждения: ");
    string confirm = Console.ReadLine()?.Trim() ?? "";
    if (confirm == "YES")
    {
        db.DeleteJournal(id);
        Console.WriteLine(" Журнал удалён.");
    }
    else
    {
        Console.WriteLine("Удаление отменено.");
    }
}

    // ═══════════════════════════════════════════════════════════════════
    // Меню отчётов
    // ═══════════════════════════════════════════════════════════════════

    static void ReportsMenu(DatabaseManager db)
{
    string choice;
    do
    {
        Console.WriteLine("╔══════════════════════════════════════════╗");
        Console.WriteLine("║               ОТЧЁТЫ                     ║");
        Console.WriteLine("╠══════════════════════════════════════════╣");
        Console.WriteLine("║ 1 — Журналы по издательствам             ║");
        Console.WriteLine("║ 2 — Количество журналов по издательствам ║");
        Console.WriteLine("║ 3 — Средний тираж по издательствам       ║");
        Console.WriteLine("║ 0 — Назад                                ║");
        Console.WriteLine("╚══════════════════════════════════════════╝");
        Console.Write("Ваш выбор: ");

        choice = Console.ReadLine()?.Trim() ?? "";
        Console.WriteLine();

        switch (choice)
        {
            case "1": Report1_JournalsWithPublishers(db); break;
            case "2": Report2_CountByPublisher(db); break;
            case "3": Report3_AvgCirculationByPublisher(db); break;
            case "0": break;
            default: Console.WriteLine("Неверный пункт."); break;
        }
    } while (choice != "0");
}

// Отчёт 1: Журналы с названиями издательств (JOIN)
static void Report1_JournalsWithPublishers(DatabaseManager db)
{
    new ReportBuilder(db)
        .Query(@"
            SELECT j.journal_name, p.publisher_name, j.circulation_k
            FROM journal j
            JOIN publisher p ON j.publisher_id = p.publisher_id
            ORDER BY j.journal_name")
        .Title("ЖУРНАЛЫ ПО ИЗДАТЕЛЬСТВАМ")
        .Header("Название журнала", "Издательство", "Тираж (тыс. экз.)")
        .ColumnWidths(25, 20, 18)
        .Numbered()
        .Print();
}

// Отчёт 2: Количество журналов по издательствам (GROUP BY + COUNT)
static void Report2_CountByPublisher(DatabaseManager db)
{
    new ReportBuilder(db)
        .Query(@"
            SELECT p.publisher_name, COUNT(*) AS cnt
            FROM journal j
            JOIN publisher p ON j.publisher_id = p.publisher_id
            GROUP BY p.publisher_name
            ORDER BY cnt DESC")
        .Title("КОЛИЧЕСТВО ЖУРНАЛОВ ПО ИЗДАТЕЛЬСТВАМ")
        .Header("Издательство", "Количество журналов")
        .ColumnWidths(30, 20)
        .Footer("Всего журналов")
        .Print();
}

// Отчёт 3: Средний тираж по издательствам (GROUP BY + AVG)
static void Report3_AvgCirculationByPublisher(DatabaseManager db)
{
    Console.WriteLine("Выберите способ вывода отчёта:");
    Console.WriteLine("1 — Вывести на экран");
    Console.WriteLine("2 — Сохранить в файл");
    Console.Write("Ваш выбор: ");

    string choice = Console.ReadLine()?.Trim() ?? "";

    var report = new ReportBuilder(db)
        .Query(@"
            SELECT p.publisher_name, ROUND(AVG(j.circulation_k), 1) AS avg_circ
            FROM journal j
            JOIN publisher p ON j.publisher_id = p.publisher_id
            GROUP BY p.publisher_name
            ORDER BY avg_circ DESC")
        .Title("СРЕДНИЙ ТИРАЖ ПО ИЗДАТЕЛЬСТВАМ")
        .Header("Издательство", "Средний тираж (тыс. экз.)")
        .ColumnWidths(30, 25);

    if (choice == "2")
    {
        string filePath = Path.Combine(AppContext.BaseDirectory, "avg_circulation_report.txt");
        report.SaveToFile(filePath);
    }
    else
    {
        report.Print();
    }
}
