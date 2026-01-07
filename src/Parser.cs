using System;
using System.Collections.Generic;

class Parser
{
    private readonly CommandLibrary commandLibrary;
    private readonly MenuSystem menuSystem;

    public Parser()
    {
        commandLibrary = new CommandLibrary();
        menuSystem = new MenuSystem(commandLibrary.GetCommandTypes());
    }

    public Command GetCommand(Room currentRoom = null)
    {
        Console.WriteLine("\nPress any key to open command menu...");
        Console.ReadKey(true);

        var selectedCommand = menuSystem.GetSelection();
        if (selectedCommand == CommandType.Unknown)
        {
            Console.WriteLine("Command cancelled.");
            return new Command(CommandType.Unknown);
        }

        string parameter = null;
        if (selectedCommand.RequiresParameter() && selectedCommand == CommandType.Go)
        {
            parameter = GetDirectionFromMenu(currentRoom);
            if (parameter == null)
            {
                Console.WriteLine("Direction selection cancelled.");
                return new Command(CommandType.Unknown);
            }
        }

        return new Command(selectedCommand, parameter);
    }

    private string GetDirectionFromMenu(Room currentRoom)
    {
        if (currentRoom == null)
        {
            Console.WriteLine("Error: No current room available.");
            return null;
        }

        var directionMenu = new DirectionMenuSystem(currentRoom.GetAvailableExits());
        return directionMenu.GetSelection()?.ToDirectionString();
    }

    public void PrintValidCommands()
    {
        Console.WriteLine("Your command words are:");
        Console.WriteLine(commandLibrary.GetCommandsString());
    }
}
