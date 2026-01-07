using System;
using System.Collections.Generic;
using System.Linq;

class Game
{
    private readonly Parser parser;
    private readonly Player player;
    private Room mapRoom;
    private string campusMap;

    public Game()
    {
        parser = new Parser();
        player = new Player();
        CreateRooms();
    }

    private void CreateRooms()
    {
        Room outside = new("Outside", "outside the main entrance of the university");
        Room theatre = new("Theatre", "in a lecture theatre");
        Room pub = new("Pub", "in the campus pub");
        Room lab = new("Lab", "in a computing lab");
        Room office = new("Office", "in the computing admin office");
        mapRoom = new("Map Room", "in the campus map room");
        Room kitchen = new("Kitchen", "in the pub's kitchen");
        Room cellar = new("Cellar", "in the pub's cellar");
        Room backyard = new("Backyard", "in the backyard");
        Room infirmary = new("Infirmary", "in the university infirmary");

        outside.AddExit(Direction.East, ExitProvider.Normal(theatre));
        outside.AddExit(Direction.South, ExitProvider.Normal(lab));
        outside.AddExit(Direction.West, ExitProvider.Normal(pub));

        theatre.AddExit(Direction.West, ExitProvider.Normal(outside));
        theatre.AddExit(Direction.South, ExitProvider.Normal(backyard));

        pub.AddExit(Direction.East, ExitProvider.Normal(outside));
        pub.AddExit(Direction.South, ExitProvider.Normal(kitchen));

        lab.AddExit(Direction.North, ExitProvider.Normal(outside));
        lab.AddExit(Direction.East, ExitProvider.Normal(office));
        lab.AddExit(Direction.South, ExitProvider.Healing(infirmary, 5, 15));

        office.AddExit(Direction.West, ExitProvider.Normal(lab));
        office.AddExit(Direction.East, ExitProvider.Normal(mapRoom));

        mapRoom.AddExit(Direction.West, ExitProvider.Normal(office));

        kitchen.AddExit(Direction.Down, ExitProvider.Hazardous(cellar, 1, 10));
        kitchen.AddExit(Direction.North, ExitProvider.Normal(pub));
        kitchen.AddExit(Direction.East, ExitProvider.Normal(backyard));

        cellar.AddExit(Direction.Up, ExitProvider.Hazardous(kitchen, 1, 8));

        backyard.AddExit(Direction.West, ExitProvider.Hazardous(kitchen, 1, 5));
        backyard.AddExit(Direction.North, ExitProvider.Normal(theatre));

        infirmary.AddExit(Direction.North, ExitProvider.Normal(lab));

        Room[] rooms =
        {
            outside, theatre, pub, lab, office, mapRoom, kitchen, cellar, backyard, infirmary
        };

        campusMap = BuildMapDescription(rooms);

        player.CurrentRoom = outside;
    }

    private static string BuildMapDescription(IEnumerable<Room> rooms)
    {
        var roomLines = rooms
            .OrderBy(r => r.Name)
            .Select(r =>
            {
                var exitInfos = r.GetExitInfos().ToList();
                if (!exitInfos.Any()) return $"{r.Name}: no exits";

                var exitsText = exitInfos
                    .OrderBy(info => info.direction.ToDirectionString())
                    .Select(info => $"{info.direction.ToDirectionString()} -> {info.destination.Name}");

                return $"{r.Name}: {string.Join(", ", exitsText)}";
            });

        return "Campus Map:\n" + string.Join("\n", roomLines);
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
        Console.WriteLine(GetRoomDescription(player.CurrentRoom));
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

    private void Look()
    {
        bool showMap = player.CurrentRoom == mapRoom;
        Console.WriteLine(GetRoomDescription(player.CurrentRoom, showMap));
    }

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

        Exit exit = player.CurrentRoom.GetExit(direction.Value);
        if (exit == null)
        {
            Console.WriteLine($"There is no door to {command.Parameter}!");
            return;
        }

        exit.Traverse(player);
        player.IncrementMoveCounter();
        Console.WriteLine(GetRoomDescription(player.CurrentRoom));
    }

    private string GetRoomDescription(Room room, bool includeMap = false)
    {
        var description = room.GetLongDescription();

        if (includeMap && room == mapRoom && !string.IsNullOrEmpty(campusMap))
        {
            return $"{description}\n\n{campusMap}";
        }

        return description;
    }
}
