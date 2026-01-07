using System;

class Game
{
    private readonly Parser parser;
    private readonly Player player;

    public Game()
    {
        parser = new Parser();
        player = new Player();
        CreateRooms();
    }

    private void CreateRooms()
    {
        Room outside = new("outside the main entrance of the university");
        Room theatre = new("in a lecture theatre");
        Room pub = new("in the campus pub");
        Room lab = new("in a computing lab");
        Room office = new("in the computing admin office");
        Room kitchen = new("in the pub's kitchen");
        Room cellar = new("in the pub's cellar");
        Room backyard = new("in the backyard");

        outside.AddExit("east", theatre);
        outside.AddExit("south", lab);
        outside.AddExit("west", pub);

        theatre.AddExit("west", outside);
        theatre.AddExit("south", backyard);

        pub.AddExit("east", outside);
        pub.AddExit("south", kitchen);

        lab.AddExit("north", outside);
        lab.AddExit("east", office);

        office.AddExit("west", lab);

        kitchen.AddHazardousExit("down", cellar, 1, 10);
        kitchen.AddExit("north", pub);
        kitchen.AddExit("east", backyard);

        cellar.AddHazardousExit("up", kitchen, 1, 8);

        backyard.AddHazardousExit("west", kitchen, 1, 5);
        backyard.AddExit("north", theatre);

        player.CurrentRoom = outside;
    }

    public void Play()
    {
        PrintWelcome();

        bool finished = false;
        while (!finished)
        {
            Command command = parser.GetCommand(player.CurrentRoom);
            if (command.IsUnknown()) continue;

            finished = ProcessCommand(command);

            if (!player.IsAlive)
            {
                Console.WriteLine("\n=== Game Over ===");
                Console.WriteLine("Your health has reached zero!");
                finished = true;
            }
        }
        Console.WriteLine("\nThank you for playing.");
        Console.WriteLine("Press [Enter] to continue.");
        Console.ReadLine();
    }


    private void PrintWelcome()
    {
        Console.WriteLine();
        Console.WriteLine("Welcome to Zuul!");
        Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
        Console.WriteLine("Type 'help' if you need help.");
        Console.WriteLine();
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
    }

    private bool ProcessCommand(Command command)
    {
        return command.Type switch
        {
            CommandType.Help => ExecuteCommand(PrintHelp),
            CommandType.Go => ExecuteCommand(() => GoRoom(command)),
            CommandType.Quit => true,
            CommandType.Look => ExecuteCommand(Look),
            CommandType.Health => ExecuteCommand(SeeHealth),
            CommandType.Unknown => false,
            _ => ExecuteCommand(() => Console.WriteLine("Command not recognized."))
        };
    }

    private bool ExecuteCommand(Action action)
    {
        action();
        return false;
    }

    private void Look() => Console.WriteLine(player.CurrentRoom.GetLongDescription());

    private void SeeHealth()
    {
        Console.WriteLine("\n=== Player Status ===");
        Console.WriteLine(player.GetHealthDisplay());
        Console.WriteLine(player.GetStatusDescription());
        Console.WriteLine($"Score: {player.Score}");
        Console.WriteLine($"Moves: {player.MovesCount}");
    }

    private void PrintHelp()
    {
        Console.WriteLine("You are lost. You are alone.");
        Console.WriteLine("You wander around at the university.");
        Console.WriteLine();
        parser.PrintValidCommands();
    }

    private void GoRoom(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Go where?");
            return;
        }

        var direction = DirectionExtensions.FromString(command.Parameter);
        if (!direction.HasValue)
        {
            Console.WriteLine($"Invalid direction: {command.Parameter}!");
            return;
        }

        Room nextRoom = player.CurrentRoom.GetExit(direction.Value);
        if (nextRoom == null)
        {
            Console.WriteLine($"There is no door to {command.Parameter}!");
            return;
        }

        if (player.CurrentRoom.IsExitHazardous(direction.Value))
        {
            var (minDamage, maxDamage) = player.CurrentRoom.GetExitDamage(direction.Value);
            int damage = Random.Shared.Next(minDamage, maxDamage + 1);
            player.TakeDamage(damage);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠️  This path is dangerous! You took {damage} damage.");
            Console.ResetColor();
        }

        player.CurrentRoom = nextRoom;
        player.IncrementMoveCounter();
        Console.WriteLine(player.CurrentRoom.GetLongDescription());
    }
}
