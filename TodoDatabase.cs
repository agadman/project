using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

// Klass som kommunicerar med SQLite-databasen för att hantera TodoItems
public class TodoDatabase
{
    private const string ConnectionString = "Data Source=./todos.db";

    public TodoDatabase()
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Todos (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Description TEXT,
                IsCompleted INTEGER NOT NULL DEFAULT 0,
                DueDate TEXT,
                CreatedAt TEXT
            );";
        tableCmd.ExecuteNonQuery();
    }

    public List<TodoItem> GetTodos()
    {
        var todos = new List<TodoItem>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Todos WHERE IsCompleted = 0 ORDER BY DueDate;";

        using var reader = selectCmd.ExecuteReader();
        while (reader.Read())
        {
            todos.Add(ReadTodo(reader));
        }

        return todos;
    }

    public void AddTodo(string title, string description, DateTime? dueDate)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var insertCmd = connection.CreateCommand();
        insertCmd.CommandText = @"
            INSERT INTO Todos (Title, Description, DueDate, CreatedAt)
            VALUES ($title, $description, $dueDate, $createdAt);";

        insertCmd.Parameters.AddWithValue("$title", title);
        insertCmd.Parameters.AddWithValue("$description", description);
        insertCmd.Parameters.AddWithValue("$createdAt", DateTime.Now.ToString("yyyy-MM-dd"));

        if (dueDate.HasValue)
            insertCmd.Parameters.AddWithValue("$dueDate", dueDate?.ToString("yyyy-MM-dd"));
        else
            insertCmd.Parameters.AddWithValue("$dueDate", DBNull.Value);

        insertCmd.ExecuteNonQuery();
    }

    public bool RemoveTodo(int id)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var deleteCmd = connection.CreateCommand();
        deleteCmd.CommandText = "DELETE FROM Todos WHERE Id = $id;";
        deleteCmd.Parameters.AddWithValue("$id", id);
        return deleteCmd.ExecuteNonQuery() > 0;
    }

    public bool markComplete(int id, bool isCompleted = true)
    {
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var completeCmd = connection.CreateCommand();
        completeCmd.CommandText = @"UPDATE Todos SET IsCompleted = $isCompleted WHERE Id = $id;";
        completeCmd.Parameters.AddWithValue("$id", id);
        completeCmd.Parameters.AddWithValue("$isCompleted", isCompleted ? 1 : 0);
        return completeCmd.ExecuteNonQuery() > 0;
    }

    public List<TodoItem> GetCompletedTodos()
    {
        var todos = new List<TodoItem>();
        using var connection = new SqliteConnection(ConnectionString);
        connection.Open();

        var selectCmd = connection.CreateCommand();
        selectCmd.CommandText = "SELECT * FROM Todos WHERE IsCompleted = 1 ORDER BY DueDate;";

        using var reader = selectCmd.ExecuteReader();
        while (reader.Read())
        {
            todos.Add(ReadTodo(reader));
        }

        return todos;
    }

    private TodoItem ReadTodo(SqliteDataReader reader)
    {
        return new TodoItem
        {
            Id = reader.GetInt32(0),
            Title = reader.GetString(1),
            Description = reader.IsDBNull(2) ? "" : reader.GetString(2),
            IsCompleted = reader.GetInt32(3) == 1,
            DueDate = reader.IsDBNull(4) ? null : DateTime.Parse(reader.GetString(4)),
            CreatedAt = reader.IsDBNull(5) ? DateTime.Now : DateTime.Parse(reader.GetString(5))
        };
    }
}