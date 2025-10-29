# Todo Console App

En C# .NET Console Application för att hantera uppgifter med SQLite-databas.
Appen erbjuder ett enkelt, färgmarkerat gränssnitt direkt i terminalen. Bra för att träna grundläggande C#-koncept som objektorientering, datalagring och konsolinteraktion.

## Funktioner
    •	Lägg till uppgifter med titel, beskrivning och förfallodatum.
    •	Ta bort uppgifter.
    •	Markera uppgifter som klara.
    •	Visa klara uppgifter i ett arkiv.
    •	Filtrera uppgifter som ska göras denna vecka.
    •	Färgmarkering i konsolen för att visa förfallna eller nära förfallna uppgifter.

## Filstruktur
| Fil | Beskrivning |
|-----|--------------|
| **Program.cs** | Startpunkt för applikationen |
| **TodoItem.cs** | Datamodell för en uppgift |
| **TodoApp.cs** | Hanterar logik, menyflöde och användarinteraktion |
| **ConsoleHelper.cs** | Hjälpfunktioner för färger, rubriker och meddelanden i konsolen |
| **TodoDatabase.cs** | Hanterar koppling och CRUD-operationer mot SQLite |

## Tekniker och koncept
    •	C# .NET 8 (Console Application)
    •	SQLite för lokal datalagring
    •	Objektorienterad programmering (OOP)
    •	Grundläggande databasoperationer (CRUD)
    •	Färg- och textformattering i konsolmiljö

## Körning
	1.	Klona eller ladda ner projektet.
	2.	Öppna det i Visual Studio eller Visual Studio Code.
	3.	Kör följande kommando i terminalen: dotnet run

## Databas
Appen använder SQLite för att spara uppgifter mellan sessioner.
Databasen skapas automatiskt första gången programmet körs och lagras som todo.db i projektmappen.

## Videolänk
https://youtu.be/kghkXIzdLxw
