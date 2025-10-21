using System;
using Microsoft.Data.Sqlite;

class TestDb
{
    static void Main()
    {
        using var connection = new SqliteConnection("Data Source=todo.db");
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = "CREATE TABLE IF NOT EXISTS Todos (Id INTEGER PRIMARY KEY, Title TEXT)";
        command.ExecuteNonQuery();

        Console.WriteLine("Databas och tabell skapad! ✅");
    }
}