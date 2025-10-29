using System;

// Klass som innehåller hjälpfunktioner för konsolutskrifter och design
public static class ConsoleHelper
{
    public static void SetupConsole()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
    }

    public static void WriteHeader(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(new string('=', text.Length));
    }

    public static void WriteSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void WriteError(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    public static void Pause()
    {
        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey(true);
    }
}
