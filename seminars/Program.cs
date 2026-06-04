using System.Text;

namespace Seminar4;

// ============================================================
// ЧАСТЬ 1. ГЕОМЕТРИЧЕСКИЕ ФИГУРЫ
// ============================================================

// ВАЖНО: Добавляем IComparable<Figure> и делаем класс public
public abstract class Figure(string type) : IComparable<Figure>
{
    public string Type { get; } = type;
    public abstract double Area { get; }

    public override string ToString() => $"{Type} площадь {Area:F2}";

    public int CompareTo(Figure? other) => other is null ? 1 : Area.CompareTo(other.Area);
}

// Все наследники тоже делаем public
public class Circle(double radius) : Figure("Круг")
{
    public override double Area => Math.PI * radius * radius;
}

public class Rectangle(double height, double width, string type = "Прямоугольник") : Figure(type)
{
    public override double Area => width * height;
}

public class Square(double size) : Rectangle(size, size, "Квадрат") { }

// ============================================================
// ЧАСТЬ 2. РАЗРЕЖЕННАЯ МАТРИЦА
// ============================================================

// Делаем интерфейс public
public interface IMatrixCheckEmpty<T>
{
    T GetEmptyElement();
    bool CheckEmptyElement(T element);
}

// Этот класс тоже public
public class FigureMatrixCheckEmpty : IMatrixCheckEmpty<Figure>
{
    public Figure GetEmptyElement() => null!;
    public bool CheckEmptyElement(Figure element) => element is null;
}

// Класс Matrix публичный
public class Matrix<T>(int maxX, int maxY, IMatrixCheckEmpty<T> checkEmpty)
{
    private readonly Dictionary<(int x, int y), T> _matrix = [];

    public T this[int x, int y]
    {
        set
        {
            CheckBounds(x, y);
            _matrix[(x, y)] = value;
        }
        get
        {
            CheckBounds(x, y);
            return _matrix.TryGetValue((x, y), out var element) ? element : checkEmpty.GetEmptyElement();
        }
    }

    private void CheckBounds(int x, int y)
    {
        if (x < 0 || x >= maxX)
            throw new ArgumentOutOfRangeException(nameof(x), $"x={x} выходит за границы");
        if (y < 0 || y >= maxY)
            throw new ArgumentOutOfRangeException(nameof(y), $"y={y} выходит за границы");
    }

    public int ColumnWidth { get; set; } = 20;

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int y = 0; y < maxY; y++)
        {
            sb.Append('|');
            for (int x = 0; x < maxX; x++)
            {
                var cell = this[x, y];
                string cellStr = !checkEmpty.CheckEmptyElement(cell) ? cell?.ToString() ?? "null" : "-";
                sb.Append(cellStr.PadRight(ColumnWidth)).Append('|');
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}

// ============================================================
// ЧАСТЬ 3. СПИСОК И СТЕК (БЕЗ СТАНДАРТНЫХ КОЛЛЕКЦИЙ)
// ============================================================

// Узел списка - public
public class SimpleListItem<T>(T data)
{
    public T Data { get; set; } = data;
    public SimpleListItem<T>? Next { get; set; }
}

// Список - public
// ВАЖНО: добавляем where T : IComparable<T> для правильной типизации
public class SimpleList<T> : IEnumerable<T> where T : IComparable<T>
{
    protected SimpleListItem<T>? first;
    protected SimpleListItem<T>? last;
    public int Count { get; protected set; }

    public void Add(T element)
    {
        var newItem = new SimpleListItem<T>(element);
        Count++;

        if (last is null)
        {
            first = newItem;
            last = newItem;
        }
        else
        {
            last.Next = newItem;
            last = newItem;
        }
    }

    public SimpleListItem<T> GetItem(int number)
    {
        if (number < 0 || number >= Count)
            throw new IndexOutOfRangeException($"Индекс {number} выходит за границы списка");

        var current = first;
        for (int i = 0; i < number; i++)
            current = current!.Next;

        return current!;
    }

    public T Get(int number) => GetItem(number).Data;

    public IEnumerator<T> GetEnumerator()
    {
        var current = first;
        while (current is not null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    public void Sort() => Sort(0, Count - 1);

    private void Sort(int low, int high)
    {
        if (low >= high) return;

        int i = low, j = high;
        T x = Get((low + high) / 2);

        do
        {
            while (Get(i).CompareTo(x) < 0) i++;
            while (Get(j).CompareTo(x) > 0) j--;

            if (i <= j)
            {
                Swap(i, j);
                i++;
                j--;
            }
        } while (i <= j);

        Sort(low, j);
        Sort(i, high);
    }

    private void Swap(int i, int j)
    {
        var ci = GetItem(i);
        var cj = GetItem(j);
        (ci.Data, cj.Data) = (cj.Data, ci.Data);
    }
}

// Стек - public
public class SimpleStack<T> : SimpleList<T> where T : IComparable<T>
{
    public void Push(T element) => Add(element);

    public T Pop()
    {
        if (Count == 0)
            return default!;

        T result;

        if (Count == 1)
        {
            result = first!.Data;
            first = null;
            last = null;
        }
        else
        {
            var newLast = GetItem(Count - 2);
            result = newLast.Next!.Data;
            last = newLast;
            newLast.Next = null;
        }

        Count--;
        return result;
    }
}

// ============================================================
// ГЛАВНАЯ ПРОГРАММА
// ============================================================

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        // ----- ЧАСТЬ 1. ГЕОМЕТРИЧЕСКИЕ ФИГУРЫ -----
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║     ЧАСТЬ 1. ГЕОМЕТРИЧЕСКИЕ ФИГУРЫ   ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        Rectangle rect = new(5, 4);
        Square square = new(5);
        Circle circle = new(5);

        Console.WriteLine(rect);
        Console.WriteLine(square);
        Console.WriteLine(circle);

        // ----- ЧАСТЬ 2. РАЗРЕЖЕННАЯ МАТРИЦА -----
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║     ЧАСТЬ 2. РАЗРЕЖЕННАЯ МАТРИЦА     ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        Matrix<Figure> matrix = new(3, 3, new FigureMatrixCheckEmpty());
        matrix[0, 0] = rect;
        matrix[1, 1] = square;
        matrix[2, 2] = circle;

        Console.WriteLine(matrix);

        // ----- ЧАСТЬ 3. СПИСОК И СОРТИРОВКА -----
        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║     ЧАСТЬ 3. СПИСОК И СОРТИРОВКА     ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        SimpleList<Figure> list = new();
        list.Add(circle);
        list.Add(rect);
        list.Add(square);

        Console.WriteLine("Список ДО сортировки (по площади):");
        foreach (var f in list)
            Console.WriteLine($"  {f}");

        list.Sort();

        Console.WriteLine("\nСписок ПОСЛЕ сортировки (по площади):");
        foreach (var f in list)
            Console.WriteLine($"  {f}");

        // ----- ЧАСТЬ 3. СТЕК -----
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║        ЧАСТЬ 3. СТЕК (LIFO)          ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        SimpleStack<Figure> stack = new();
        stack.Push(rect);
        stack.Push(square);
        stack.Push(circle);

        Console.WriteLine("Извлечение из стека (LIFO - последний зашёл, первый вышел):");
        while (stack.Count > 0)
        {
            Console.WriteLine($"  Извлечено: {stack.Pop()}");
        }

        Console.WriteLine("\n✅ Программа завершена успешно!");
    }
}