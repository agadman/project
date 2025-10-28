using System;
using System.Linq;
using System.Collections.Generic;

// Klass som hanterar logiken, alltså det som användaren interagerar med
public class TodoApp
{
    private readonly TodoDatabase _db = new();
    private List<TodoItem> _cachedTodos = new(); 

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            ConsoleHelper.WriteHeader("ATT GÖRA LISTA");

            _cachedTodos = _db.GetTodos().OrderBy(t => t.DueDate ?? DateTime.MaxValue).ToList();

            if (_cachedTodos.Count == 0)
            {
                Console.WriteLine("Inga uppgifter att göra ännu.\n");
            }
            else
            {
                for (int i = 0; i < _cachedTodos.Count; i++)
                {
                    var todo = _cachedTodos[i];
                    Console.Write($"{i + 1}. ");
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

        Console.Write("Titel: ");
        string title = Console.ReadLine()?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(title))
        {
            ConsoleHelper.WriteError("Titel får inte vara tom.");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Beskrivning (valfritt): ");
        string description = Console.ReadLine() ?? "";

        DateTime? dueDate = null;
        Console.Write("Förfallodatum (yyyy-mm-dd eller tomt): ");
        string dateInput = Console.ReadLine() ?? "";

        if (!string.IsNullOrWhiteSpace(dateInput))
        {
            if (DateTime.TryParse(dateInput, out DateTime parsed))
                dueDate = parsed;
            else
            {
                ConsoleHelper.WriteError("Felaktigt datumformat.");
                ConsoleHelper.Pause();
                return;
            }
        }

        _db.AddTodo(title, description, dueDate);
        ConsoleHelper.WriteSuccess("Uppgift tillagd!");
        ConsoleHelper.Pause();
    }

    private void RemoveTodo()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("TA BORT UPPGIFT");

        if (_cachedTodos.Count == 0)
        {
            Console.WriteLine("Inga uppgifter att ta bort.");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Ange numret på uppgiften: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index > 0 && index <= _cachedTodos.Count)
        {
            int id = _cachedTodos[index - 1].Id;
            bool removed = _db.RemoveTodo(id);
            if (removed)
                ConsoleHelper.WriteSuccess("Uppgift borttagen!");
            else
                ConsoleHelper.WriteError("Kunde inte ta bort uppgiften.");
        }
        else
        {
            ConsoleHelper.WriteError("Felaktigt nummer.");
        }
        ConsoleHelper.Pause();
    }

    private void MarkAsComplete()
    {
        Console.Clear();
        ConsoleHelper.WriteHeader("MARKERA SOM KLAR");

        if (_cachedTodos.Count == 0)
        {
            Console.WriteLine("Inga uppgifter att markera.");
            ConsoleHelper.Pause();
            return;
        }

        Console.Write("Ange numret på uppgiften: ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index > 0 && index <= _cachedTodos.Count)
        {
            int id = _cachedTodos[index - 1].Id;
            bool success = _db.markComplete(id);
            if (success)
                ConsoleHelper.WriteSuccess("Uppgift markerad som klar!");
            else
                ConsoleHelper.WriteError("Kunde inte markera uppgiften.");
        }
        else
        {
            ConsoleHelper.WriteError("Felaktigt nummer.");
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
            for (int i = 0; i < completed.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                DisplayTodoItem(completed[i]);
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
            for (int i = 0; i < todos.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                DisplayTodoItem(todos[i]);
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

            Console.WriteLine($"Klar senast: {todo.DueDate:yyyy-MM-dd}");
            Console.ForegroundColor = ConsoleColor.White;
        }
        Console.WriteLine();
    }
}
