using Microsoft.Data.Sqlite;
using System;

public class TodoDatabase
{
    private const string ConnectionString = "Data Source=todos.db";

    public TodoDatabase()
    {
       // Skapar databasen och tabellen om de inte finns
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Todos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                IsCompleted INTEGER NOT NULL Default 0,
                DueDate TEXT
            );";
        tableCmd.ExecuteNonQuery();
    }
}