# Todo Console App

En enkel konsolapplikation för att hantera uppgifter (To-Do List) i C# med .NET och SQLite.

## Funktioner
- Lägg till uppgifter med titel, beskrivning och förfallodatum.
- Ta bort uppgifter.
- Markera uppgifter som klara.
- Visa klara uppgifter i ett arkiv.
- Filtrera uppgifter som ska göras denna vecka.
- Färgmarkering i konsolen för att visa förfallna eller nära förfallna uppgifter.

## Filstruktur
- `Program.cs` – Startpunkt för applikationen.
- `TodoItem.cs` – Datamodellen för en uppgift.
- `TodoApp.cs` – Logik och användarflöde.
- `ConsoleHelper.cs` – Hjälpfunktioner för utskrift och färger i konsolen.
- `TodoDatabase.cs` – Hanterar SQLite-databasen.

## Körning
1. Öppna projektet i Visual Studio eller VS Code.  
2. Kör kommandot: dotnet run