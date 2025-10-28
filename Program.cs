// startar programmet och sätter upp konsolen
class Program
{
    static void Main(string[] args)
    {
        ConsoleHelper.SetupConsole();
        new TodoApp().Run();
    }
}
