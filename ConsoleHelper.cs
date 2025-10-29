using System;

// Klass som innehåller hjälpfunktioner för konsolutskrifter och design (i konsol)
public static class ConsoleHelper
{
    // Sätter upp konsolfärger och bakgrund
    public static void SetupConsole()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.Black;
        Console.Clear();
    }

    // Skriver en rubrik med cyan och linje (=) under
    public static void WriteHeader(string text)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(new string('=', text.Length));
    }

    // Skriver ett meddelande i grönt vid success
    public static void WriteSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Skriver ett meddelande i rött vid fel/varning
    public static void WriteError(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ForegroundColor = ConsoleColor.White;
    }

    // Pausar programmet tills användaren trycker på en tangent
    public static void Pause()
    {
        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey(true);
    }
}
