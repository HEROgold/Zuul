public class Parser
{
    private readonly CommandLibrary commandLibrary;
    private readonly MenuSystem menuSystem;

    public Parser()
    {
        commandLibrary = new CommandLibrary();
        menuSystem = new MenuSystem(commandLibrary.GetCommandTypes());
    }

    public Command GetCommand(Player player = null)
    {
        Console.WriteLine("\nPress any key to open command menu...");
        Console.ReadKey(true);

        var selectedCommand = menuSystem.GetSelection();
        if (selectedCommand == CommandType.Unknown)
        {
            Console.WriteLine("Command cancelled.");
            return new(CommandType.Unknown);
        }

        var parameter = selectedCommand.RequiresParameter()
            ? GetParameterForCommand(selectedCommand, player)
            : null;

        return parameter == null && selectedCommand.RequiresParameter()
            ? new Command(CommandType.Unknown)
            : new Command(selectedCommand, parameter);
    }

    private IParameter GetParameterForCommand(CommandType commandType, Player player) => commandType switch
    {
        CommandType.Go => GetDirectionParameter(player.CurrentRoom),
        CommandType.Take => GetItemParameterFromChest(player.CurrentRoom),
        CommandType.Place or CommandType.Drop or CommandType.Use => GetItemParameterFromBackpack(player),
        CommandType.Heal or CommandType.Damage => GetNumberParameter(),
        _ => null
    };

    private static DirectionParameter? GetDirectionParameter(Room currentRoom)
    {
        if (currentRoom == null)
        {
            Console.WriteLine("Error: No current room available.");
            return null;
        }

        var directionMenu = new DirectionMenuSystem(currentRoom.GetAvailableExits());
        var direction = directionMenu.GetSelection();

        if (!direction.HasValue)
        {
            Console.WriteLine("Direction selection cancelled.");
            return null;
        }

        return new DirectionParameter(direction.Value);
    }

    private static ItemParameter? GetItemParameterFromChest(Room currentRoom)
    {
        var item = GetItemFromChest(currentRoom);
        if (!item.HasValue)
        {
            Console.WriteLine("Item selection cancelled.");
            return null;
        }
        return new ItemParameter(item.Value);
    }

    private static ItemParameter? GetItemParameterFromBackpack(Player player)
    {
        var item = GetItemFromBackpack(player);
        if (!item.HasValue)
        {
            Console.WriteLine("Item selection cancelled.");
            return null;
        }
        return new ItemParameter(item.Value);
    }

    private static NumberParameter? GetNumberParameter()
    {
        Console.Write("Enter amount: ");
        if (int.TryParse(Console.ReadLine(), out int amount))
        {
            return new NumberParameter(amount);
        }

        Console.WriteLine("Invalid number.");
        return null;
    }

    private static Item? GetItemFromChest(Room currentRoom)
    {
        if (currentRoom?.Chest == null || currentRoom.Chest.Items.Count == 0)
        {
            Console.WriteLine("No items available in this room.");
            Console.ReadKey(true);
            Console.Clear();
            return null;
        }

        var itemMenu = new ItemMenuSystem(currentRoom.Chest.Items.Values);
        return itemMenu.GetSelection("Take Item from Chest");
    }

    private static Item? GetItemFromBackpack(Player player)
    {
        if (player?.Backpack == null || player.Backpack.Items.Count == 0)
        {
            Console.WriteLine("Your backpack is empty.");
            Console.ReadKey(true);
            Console.Clear();
            return null;
        }

        var itemMenu = new ItemMenuSystem(player.Backpack.Items.Values);
        return itemMenu.GetSelection("Select Item from Backpack");
    }

    public void PrintValidCommands()
    {
        Console.WriteLine("Your commands are:");
        Console.WriteLine(commandLibrary.GetCommandsString());
    }
}
