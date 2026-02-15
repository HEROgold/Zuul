using System;
using System.Collections.Generic;
using System.Linq;

class Game
{
    private readonly Parser parser;
    private readonly Player player;
    private Room mapRoom;
    private Room winRoom;
    private Room nurgleRoom;
    private string campusMap;
    private Random rndnum = new Random();

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

        // Add items to rooms
        Item bandage = new Item(10, "bandage");
        Item medkit = new Item(40, "medkit");
        Item key = new Item(5, "key");
        Item metalrod = new Item(30, "metalrod");
        Item piston = new Item(50, "piston");
        Item ducttape = new Item(5, "ducttape");

        outside.Chest.Put("bandage", bandage);
        pub.Chest.Put("medkit", medkit);
        cellar.Chest.Put("key", key);
        backyard.Chest.Put("metalrod", metalrod);
        lab.Chest.Put("piston", piston);
        kitchen.Chest.Put("ducttape", ducttape);

        // Add enemies to rooms
        Enemy mountofflesh = new Enemy(100, "mountofflesh", 10, kitchen);
        kitchen.AddEnemy(mountofflesh);

        // Set special rooms
        winRoom = theatre;
        nurgleRoom = cellar;

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

            // Check for win condition
            if (player.CurrentRoom == winRoom)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nYou Won\n");
                finished = true;
            }

            // Check for death
            if (!player.IsAlive)
            {
                Console.WriteLine("\n=== Game Over ===");
                Console.WriteLine("Your health has reached zero!");
                finished = true;
            }

            // Check if devoured by Nurgle
            if (player.CurrentRoom == nurgleRoom)
            {
                Console.WriteLine("Why did I do this. I should have known it would kill me.\n");
                player.Sepukku();
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
            CommandType.Take => ExecuteCommand(() => TakeItem(command)),
            CommandType.Place => ExecuteCommand(() => PlaceItem(command)),
            CommandType.Backpack => ExecuteCommand(CheckBackpack),
            CommandType.Use => ExecuteCommand(() => UseItem(command)),
            CommandType.Craft => ExecuteCommand(() => CraftItem(command)),
            CommandType.Attack => ExecuteCommand(PlayerAttack),
            CommandType.Heal => ExecuteCommand(() => HealPlayer(command)),
            CommandType.Damage => ExecuteCommand(() => DamagePlayer(command)),
            CommandType.Die => ExecuteCommand(() => player.Sepukku()),
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
        
        // Show items in room
        if (player.CurrentRoom.Chest != null)
        {
            foreach (var (_, item) in player.CurrentRoom.Chest.getItems())
            {
                Console.WriteLine("You found:");
                Console.Write(item.Description);
                Console.Write(" - ");
                Console.WriteLine(item.Weight);
                Console.WriteLine($"Type take {item.Description} to grab the item.\n");
            }
        }
        
        Console.WriteLine(GetRoomDescription(player.CurrentRoom, showMap));
    }

    private void SeeHealth()
    {
        Console.WriteLine("\n=== Player Status ===");
        Console.WriteLine(player.GetHealthDisplay());
        Console.WriteLine(player.GetStatusDescription());
        Console.WriteLine($"Score: {player.Score}");
        Console.WriteLine($"Moves: {player.MovesCount}");
        
        // Show inventory
        Console.WriteLine("\n=== Backpack ===");
        foreach (var (_, item) in player.GetBackpack().getItems())
        {
            Console.Write(item.Description);
            Console.Write(" - ");
            Console.WriteLine(item.Weight);
        }
        player.CheckWeight();
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

        // Check for locked rooms
        if (exit.Destination.GetLock())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("This door is locked.");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        // Check for Nurgle lock
        if (exit.Destination.GetNurgleLock())
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("It's mouth is closed maybe I can open it.");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        // Check for enemies
        if (player.CurrentRoom.enemy != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is a enemy in this room. You must defeat it first!");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        exit.Traverse(player);
        player.IncrementMoveCounter();
        player.LowHp();
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

    private void PlayerAttack()
    {
        if (player.CurrentRoom.enemy == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("There is nothing here to attack.\n");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        int playerAttack = 0;
        int dodgenrplayer = rndnum.Next(1, 11);
        if (dodgenrplayer == 1)
        {
            playerAttack = 0; // Player missed
        }
        else
        {
            playerAttack = rndnum.Next(15, 26);
            player.CurrentRoom.enemy.DamageEnemy(playerAttack);
        }

        player.EnemyAttack();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"You got hit for {player.enemyAttack} damage. You got {player.Health}HP left\n");
        Console.ForegroundColor = ConsoleColor.Green;
        
        if (player.CurrentRoom.enemy.CurrentHealthEnemy > 0)
        {
            Console.WriteLine($"You hit the enemy for {playerAttack} damage. It still has {player.CurrentRoom.enemy.CurrentHealthEnemy}HP remaining\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("You slayed the thing.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        if (!player.CurrentRoom.enemy.EnemyIsAlive())
        {
            player.CurrentRoom.enemy = null;
        }
    }

    private void TakeItem(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Take what?");
            return;
        }
        player.TakeFromChest(command.Parameter);
    }

    private void PlaceItem(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Place what?");
            return;
        }
        player.PutInChest(command.Parameter);
    }

    private void CheckBackpack()
    {
        Console.WriteLine("\n=== Backpack ===");
        foreach (var (_, item) in player.GetBackpack().getItems())
        {
            Console.Write(item.Description);
            Console.Write(" - ");
            Console.WriteLine(item.Weight);
        }
        player.CheckWeight();
    }

    private void UseItem(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Use what?");
            return;
        }
        player.UseItem(command.Parameter);
    }

    private void CraftItem(Command command)
    {
        // For crafting, we need 3 items - for now just show message
        Console.WriteLine("Craft command needs 3 items. Use the menu to specify items.");
        // TODO: Enhance menu system to support multiple parameters
    }

    private void HealPlayer(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Heal how much?");
            return;
        }

        if (int.TryParse(command.Parameter, out int amount))
        {
            if (player.Health <= player.MaxHealth - amount)
            {
                player.Heal(amount);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You healed! Your health is now: {player.Health}HP");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("You aren't all that injured are you?");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please add a valid number...");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    private void DamagePlayer(Command command)
    {
        if (!command.HasParameter())
        {
            Console.WriteLine("Damage how much?");
            return;
        }

        if (int.TryParse(command.Parameter, out int amount))
        {
            if (player.Health >= amount)
            {
                player.TakeDamage(amount);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You took {amount} damage! Your health is now: {player.Health}HP");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("That would kill u.");
            }
        }
        else
        {
            Console.WriteLine("Please add a valid number...");
        }
    }
}
