using System;
using System.Linq;
using System.Collections.Generic;

// Klass som hanterar logiken, alltså det som användaren interagerar med
public class TodoApp
{
    private readonly TodoDatabase _db = new();

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("ATT GÖRA LISTA");

            var todos = _db.GetTodos().OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();

            if (todos.Count == 0)
            {
                Console.WriteLine("Inga uppgifter att göra ännu.\n");
            }
            else
            {
                foreach (var todo in todos)
                {
                    Console.Write($"ID {todo.Id}: ");
                    DisplayTodoItem(todo);
                }
            }

            ShowMenu();

            char choice = Console.ReadKey(true).KeyChar;
            HandleMenuChoice(choice);
        }
    }

    private void ShowMenu()
    {
        ConsoleHelper.WriteHeader("MENY");
        Console.WriteLine("1. Lägg till uppgift");
        Console.WriteLine("2. Ta bort uppgift");
        Console.WriteLine("3. Markera som klar");
        Console.WriteLine("4. Arkiv");
        Console.WriteLine("5. Visa uppgifter denna vecka");
        Console.WriteLine("6. Avsluta\n");
        Console.Write("Välj ett alternativ: ");
    }

    private void HandleMenuChoice(char choice)
    {
        switch (choice)
        {
            case '1': AddTodo(); break;
            case '2': RemoveTodo(); break;
            case '3': MarkAsComplete(); break;
            case '4': ShowArchive(); break;
            case '5': FilterWeek(); break;
            case '6': Environment.Exit(0); break;
            default:
                ConsoleHelper.WriteError("Ogiltigt val. Försök igen.");
                ConsoleHelper.Pause();
                break;
        }
    }

    private void AddTodo()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("LÄGG TILL UPPGIFT");
        Console.WriteLine("(Tryck Enter utan att skriva något för att avbryta)\n");

        Console.Write("Titel: ");
        string title = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(title))
        {
            ConsoleHelper.WriteSuccess("Avbrutet.");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Beskrivning (valfritt): ");
        string description = Console.ReadLine() ?? "";

        DateTime? dueDate = null;
        Console.Write("Klar senast (yyyy-mm-dd eller tomt): ");
        string dateInput = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(dateInput))
        {
            // Ingen förfallodag - fortsätt som vanligt
        }
        else if (!DateTime.TryParse(dateInput, out DateTime parsed))
        {
            ConsoleHelper.WriteError("Felaktigt datumformat.");
            ConsoleHelper.Pause();
            return;
        }
        else
        {
            dueDate = parsed;
        }

        _db.AddTodo(title, description, dueDate);
        ConsoleHelper.WriteSuccess("Uppgift tillagd!");
        ConsoleHelper.Pause();
    }

    private void RemoveTodo()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("TA BORT UPPGIFT");

        var todos = _db.GetTodos().OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();
        if (todos.Count == 0)
        {
            Console.WriteLine("Inga uppgifter att ta bort.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine("Mina uppgifter:");
        foreach (var todo in todos)
        {
            Console.Write($"ID {todo.Id}: ");
            Console.WriteLine($"{todo.Title}");
            if (todo.DueDate != null)
                Console.WriteLine($"Klar senast: {todo.DueDate:yyyy-MM-dd}");
            Console.WriteLine();
        }

        Console.Write("Ange ID på uppgiften som ska tas bort (eller 0 för att avbryta): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input == "0")
        {
            ConsoleHelper.WriteSuccess("Avbrutet.");
            ConsoleHelper.Pause();
            return;
        }

        if (int.TryParse(input, out int id))
        {
            bool removed = _db.RemoveTodo(id);
            if (removed)
                ConsoleHelper.WriteSuccess("Uppgift borttagen!");
            else
                ConsoleHelper.WriteError("Kunde inte hitta uppgiften med det ID:t.");
        }
        else
        {
            ConsoleHelper.WriteError("Felaktigt ID.");
        }
        ConsoleHelper.Pause();
    }

    private void MarkAsComplete()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("MARKERA SOM KLAR");

        var todos = _db.GetTodos().OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();

        if (todos.Count == 0)
        {
            Console.WriteLine("Inga uppgifter att markera.");
            ConsoleHelper.Pause();
            return;
        }

        Console.WriteLine("Mina uppgifter:");
        foreach (var todo in todos)
        {
            Console.Write($"ID {todo.Id}: ");
            Console.WriteLine($"{todo.Title}");
            if (todo.DueDate != null)
                Console.WriteLine($"Klar senast: {todo.DueDate:yyyy-MM-dd}");
            Console.WriteLine();
        }

        Console.Write("Ange ID på uppgiften du vill markera som klar (eller 0 för att avbryta): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || input == "0")
        {
            ConsoleHelper.WriteSuccess("Avbrutet.");
            ConsoleHelper.Pause();
            return;
        }

        if (int.TryParse(input, out int id))
        {
            bool success = _db.markComplete(id);
            if (success)
                ConsoleHelper.WriteSuccess("Uppgift markerad som klar!");
            else
                ConsoleHelper.WriteError("Kunde inte markera uppgiften (kontrollera ID).");
        }
        else
        {
            ConsoleHelper.WriteError("Felaktig inmatning, ange ett giltigt ID.");
        }

        ConsoleHelper.Pause();
    }

    private void ShowArchive()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("ARKIV - KLARA UPPGIFTER");

        var completed = _db.GetCompletedTodos();
        if (completed.Count == 0)
        {
            Console.WriteLine("Inga klara uppgifter ännu.");
        }
        else
        {
            foreach (var todo in completed)
            {
                Console.Write($"ID {todo.Id}: ");
                DisplayTodoItem(todo);
            }
        }
        ConsoleHelper.Pause();
    }

    private void FilterWeek()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("UPPGIFTER DENNA VECKA");

        var todos = _db.GetTodos()
                       .Where(t => t.DueDate != null && t.DueDate.Value <= DateTime.Now.AddDays(7))
                       .OrderBy(t => t.DueDate)
                       .ToList();

        if (todos.Count == 0)
            Console.WriteLine("Inga uppgifter inom en vecka.");
        else
        {
            foreach (var todo in todos)
            {
                Console.Write($"ID {todo.Id}: ");
                DisplayTodoItem(todo);
            }
        }

        ConsoleHelper.Pause();
    }

    private void DisplayTodoItem(TodoItem todo)
    {
        Console.WriteLine($"{todo.Title}");
        if (!string.IsNullOrEmpty(todo.Description))
            Console.WriteLine($"   {todo.Description}");
        if (todo.DueDate != null)
        {
            if (todo.DueDate < DateTime.Now)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (todo.DueDate < DateTime.Now.AddDays(3))
                Console.ForegroundColor = ConsoleColor.Yellow;
            else
                Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"   Klar senast: {todo.DueDate:yyyy-MM-dd}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine();
    }
}