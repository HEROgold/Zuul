public class CommandExecutor(Player player, CraftingSystem crafting, Parser parser)
{
    public bool Execute(Command command) => command.Type switch
    {
        CommandType.Help => ExecuteAndContinue(PrintHelp),
        CommandType.Go => ExecuteAndContinue(() => GoRoom(command)),
        CommandType.Quit => true,
        CommandType.Look => ExecuteAndContinue(Look),
        CommandType.Health => ExecuteAndContinue(player.ShowStatus),
        CommandType.Take => ExecuteAndContinue(() => InventoryManager.TransferFromChest(player, (command.Parameter as ItemParameter?)?.Item)),
        CommandType.Place => ExecuteAndContinue(() => InventoryManager.TransferToChest(player, (command.Parameter as ItemParameter?)?.Item)),
        CommandType.Drop => ExecuteAndContinue(() => InventoryManager.DropItem(player, (command.Parameter as ItemParameter?)?.Item)),
        CommandType.Backpack => ExecuteAndContinue(player.Backpack.Display),
        CommandType.Use => ExecuteAndContinue(() => UseItem(command)),
        CommandType.Craft => ExecuteAndContinue(() => crafting.TryCraft(player.Backpack, [], out _)),
        CommandType.Attack => ExecuteAndContinue(PlayerAttack),
        CommandType.Heal => ExecuteAndContinue(() => HealPlayer(command)),
        CommandType.Damage => ExecuteAndContinue(() => DamagePlayer(command)),
        CommandType.Die => ExecuteAndContinue(player.Kill),
        CommandType.Cast => ExecuteAndContinue(() => CastSpell(command)),
        CommandType.Spells => ExecuteAndContinue(player.ShowSpells),
        CommandType.Unknown => false,
        _ => ExecuteAndContinue(() => Console.WriteLine("Command not recognized."))
    };

    private static bool ExecuteAndContinue(Action action)
    {
        action();
        return false;
    }

    private void Look()
    {
        // Show items in room
        if (player.CurrentRoom.Chest?.Items.Count > 0)
        {
            foreach (var item in player.CurrentRoom.Chest.Items.Values)
            {
                Console.WriteLine($"You found: {item.Description} - {item.Weight}kg");
                Console.WriteLine($"Use `take` to grab {item.Description}.\n");
            }
        }

        bool showMap = player.CurrentRoom == GameMap.MapRoom;
        Console.WriteLine(player.CurrentRoom.GetDescription(showMap, GameMap.CampusMap));
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

        if (command.Parameter is not DirectionParameter dirParam)
        {
            Console.WriteLine("Invalid direction parameter!");
            return;
        }

        var direction = dirParam.Direction;
        Exit exit = player.CurrentRoom.GetExit(direction);
        if (exit == null)
        {
            Console.WriteLine($"There is no door to {direction.ToDirectionString()}!");
            return;
        }

        // Check for locked rooms
        if (exit.Destination.IsLocked)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("This door is locked.");
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        // Check for Nurgle lock
        if (exit.Destination.IsNurgleLocked)
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
        player.IncrementMoves();
        player.ShowLowHealthWarning();
        Console.WriteLine(player.CurrentRoom.GetDescription());
    }

    private void PlayerAttack()
    {
        player.Attack(player.CurrentRoom.enemy);
        if (player.CurrentRoom.enemy?.IsAlive == false)
            player.CurrentRoom.enemy = null;
    }

    private void UseItem(Command command)
    {
        if (command.Parameter is not ItemParameter itemParam)
        {
            Console.WriteLine("No item selected.");
            return;
        }

        var item = itemParam.Item;
        Direction? direction = null;

        // Check if item needs a direction (Key or Hydraulics)
        if (item == ItemProvider.Key || item == ItemProvider.Hydraulics)
        {
            Console.WriteLine("\nSelect direction to use item:");
            var directionMenu = new DirectionMenuSystem(player.CurrentRoom.GetAvailableExits());
            direction = directionMenu.GetSelection();

            if (!direction.HasValue)
            {
                Console.WriteLine("Direction selection cancelled.");
                player.Backpack.Put(item); // Put it back
                return;
            }
        }

        player.UseItem(item.Description, direction);
    }

    private void HealPlayer(Command command) =>
        ExecuteWithNumber(command, player.HealAmount, "No amount specified.");

    private void DamagePlayer(Command command) =>
        ExecuteWithNumber(command, player.DamageAmount, "No amount specified.");

    private void CastSpell(Command command)
    {
        if (command.Parameter is not SpellParameter spellParam)
        {
            Console.WriteLine("No spell selected.");
            return;
        }

        player.CastSpell(spellParam.Spell);
    }

    private static void ExecuteWithNumber(Command command, Action<int> action, string errorMessage)
    {
        if (command.Parameter is NumberParameter numParam)
            action(numParam.Value);
        else
            Console.WriteLine(errorMessage);
    }
}
