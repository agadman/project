using System;

class Program
{
    static void Main(string[] args)
    {
        TodoDatabase db = new TodoDatabase();

        while (true)
        {
            Console.Clear();
            Console.CursorVisible = false;

            var todos = db.GetTodos();
            Console.WriteLine("==== EJ KLARA UPPGIFTER ====\n");

            if (todos.Count == 0)
            {
                Console.WriteLine("Inga uppgifter att göra ännu.\n");
            }
            else
            {
                foreach (var todo in todos)
                {
                    string status = todo.IsCompleted ? "Klar" : "Ej klar";
                    Console.WriteLine($"{todo.Id}. {todo.Title} - {status}");
                    if (!string.IsNullOrEmpty(todo.Description))
                        Console.WriteLine($"Beskrivning: {todo.Description}");
                    if (todo.DueDate != null)
                        Console.WriteLine($"Förfallodatum: {todo.DueDate:yyyy-MM-dd}");
                    Console.WriteLine();
                }
            }

            // 2️⃣ Visa menyn
            Console.WriteLine("==== MENY ====");
            Console.WriteLine("1. Lägg till uppgift");
            Console.WriteLine("2. Ta bort uppgift");
            Console.WriteLine("3. Markera uppgift som klar");
            Console.WriteLine("4. Arkiv");
            Console.WriteLine("5. Avsluta\n");

            // 3️⃣ Be användaren välja alternativ
            Console.Write("Välj ett alternativ: ");
            var choice = Console.ReadKey(true).KeyChar;

            switch (choice)
            {
                case '1':
                    Console.CursorVisible = true;
                    Console.Clear();
                    Console.WriteLine("==== LÄGG TILL UPPGIFT ====\n");
                    Console.Write("Titel: ");
                    string title = Console.ReadLine() ?? "";
                    Console.Write("Beskrivning: ");
                    string description = Console.ReadLine() ?? "";
                    Console.Write("Förfallodatum (yyyy-mm-dd eller tomt): ");
                    string dateInput = Console.ReadLine() ?? "";
                    DateTime? dueDate = string.IsNullOrWhiteSpace(dateInput) ? null : DateTime.Parse(dateInput);

                    db.AddTodo(title, description, dueDate);
                    Console.WriteLine("\nUppgift tillagd! Tryck på valfri tangent...");
                    Console.ReadKey(true);
                    break;

                case '2':
                    Console.CursorVisible = true;
                    Console.Clear();
                    Console.WriteLine("==== TA BORT UPPGIFT ====\n");
                    Console.Write("Ange ID att ta bort: ");
                    string? idInput = Console.ReadLine();
                    if (int.TryParse(idInput, out int id))
                    {
                        db.RemoveTodo(id);
                        Console.WriteLine("🗑️ Uppgift borttagen!");
                    }
                    else
                    {
                        Console.WriteLine("Felaktigt ID.");
                    }
                    Console.WriteLine("Tryck på valfri tangent för att gå tillbaka...");
                    Console.ReadKey(true);
                    break;

                case '3':
                    Console.CursorVisible = true;
                    Console.Clear();
                    Console.WriteLine("==== MARKERA UPPGIFT SOM KLAR ====\n");
                    Console.Write("Ange ID som ska markeras som klar: ");
                    string? completeInput = Console.ReadLine();
                    if (int.TryParse(completeInput, out int completeId))
                    {
                        db.markComplete(completeId);
                        Console.WriteLine("Uppgift markerad som klar!");
                    }
                    else
                    {
                        Console.WriteLine("Felaktigt ID.");
                    }
                    Console.WriteLine("Tryck på valfri tangent för att gå tillbaka...");
                    Console.ReadKey(true);
                    break;

                case '4':
                    Console.Clear();
                    var completed = db.GetCompletedTodos();
                    Console.WriteLine("==== ARKIV (KLARA UPPGIFTER) ====\n");

                    if (completed.Count == 0)
                    {
                        Console.WriteLine("Inga klara uppgifter ännu.");
                    }
                    else
                    {
                        foreach (var todo in completed)
                        {
                            Console.WriteLine($"{todo.Id}. {todo.Title}");
                            if (!string.IsNullOrEmpty(todo.Description))
                                Console.WriteLine($"   {todo.Description}");
                            if (todo.DueDate != null)
                                Console.WriteLine($"   Förfallodatum: {todo.DueDate:yyyy-MM-dd}");
                            Console.WriteLine();
                        }
                    }
                    Console.WriteLine("Tryck på valfri tangent för att gå tillbaka...");
                    Console.ReadKey(true);
                    break;

                case '5':
                    Environment.Exit(0);
                    break;
            }
        }
    }
}