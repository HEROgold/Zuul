using System;
using System.Collections.Generic;
using System.Linq;

class MenuSystem
{
    private readonly List<CommandType> options;
    private int selectedIndex;

    public MenuSystem(List<CommandType> menuOptions) => (options, selectedIndex) = (menuOptions, 0);

    public void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Select a Command ===");
        Console.WriteLine("Use ↑/↓ arrows to navigate, SPACE/ENTER to select, ESC to cancel\n");

        for (int i = 0; i < options.Count; i++)
        {
            CommandType cmd = options[i];
            string display = $"{cmd.ToCommandString()} - {cmd.GetDescription()}";

            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($" ► {display}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"   {display}");
            }
        }
    }

    public CommandType GetSelection()
    {
        ConsoleKey keyPressed;
        do
        {
            DisplayMenu();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : options.Count - 1;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = selectedIndex < options.Count - 1 ? selectedIndex + 1 : 0;
                    break;

                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    Console.Clear();
                    return options[selectedIndex];

                case ConsoleKey.Escape:
                    Console.Clear();
                    return CommandType.Unknown;
            }
        } while (keyPressed is not ConsoleKey.Spacebar and not ConsoleKey.Enter and not ConsoleKey.Escape);

        return CommandType.Unknown;
    }

    public void ResetSelection() => selectedIndex = 0;
}
