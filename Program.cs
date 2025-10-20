using System;

class Program
{
    static void Main()
    {
       // Testar att skapa ett nytt TodoItem-objekt
        TodoItem todo = new TodoItem();
        todo.Title = "Handla";
        todo.Description = "Storhandla mat och dryck";
        todo.IsCompleted = false;
        todo.DueDate = DateTime.Now.AddDays(2); //lägger in 2 dagar från idag

        // Skriver ut egenskaperna för test
        Console.WriteLine("Titel: " + todo.Title);
        Console.WriteLine("Beskrivning: " + todo.Description);
        Console.WriteLine("Klar? " + (todo.IsCompleted ? "Ja" : "Nej"));
        Console.WriteLine("Sista datum: " + todo.DueDate?.ToString("yyyy-MM-dd"));  
    }
}
