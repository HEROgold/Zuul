class Game
{
    private static readonly Parser parser = new();
    private static readonly Player player = new();
    private static readonly CraftingSystem crafting = new();
    private static readonly CommandExecutor executor = new(player, crafting, parser);

    static Game()
    {
        player.CurrentRoom = GameMap.StartRoom;
    }

    public static void Play()
    {
        PrintWelcome();

        bool finished = false;
        while (!finished)
        {
            Command command = parser.GetCommand(player);
            if (command.IsUnknown()) continue;

            finished = executor.Execute(command) || CheckGameStatus();
        }
        Console.WriteLine("\nThank you for playing.");
        Console.WriteLine("Press [Enter] to continue.");
        Console.ReadLine();

        static bool CheckGameStatus()
        {
            // Check for win condition - must be in win room AND it must be unlocked
            if (player.CurrentRoom == GameMap.WinRoom && !GameMap.WinRoom.IsLocked)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nYou Won\n");
                Console.WriteLine("Congratulations! You crafted the hydraulics and unlocked the theatre!");
                return true;
            }

            // Check for death
            if (!player.IsAlive)
            {
                Console.WriteLine("\n=== Game Over ===");
                Console.WriteLine("Your health has reached zero!");
                return true;
            }

            // Check if devoured by Nurgle
            if (player.CurrentRoom == GameMap.NurgleRoom)
            {
                Console.WriteLine("Why did I do this. I should have known it would kill me.\n");
                player.Kill();
            }

            return false;
        }
    }

    private static void PrintWelcome()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Zuul!");
        Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
        Console.WriteLine("Use 'help' if you need help.");
        Console.WriteLine();
        Console.WriteLine(player.CurrentRoom.GetDescription());
    }
}
