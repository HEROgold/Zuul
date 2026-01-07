using System;
using System.Collections.Generic;
using System.Linq;

class DirectionMenuSystem
{
    private readonly List<Direction> availableDirections;
    private int selectedIndex;

    public DirectionMenuSystem(IEnumerable<Direction> directions) =>
        (availableDirections, selectedIndex) = (directions.ToList(), 0);

    public void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine("=== Select a Direction ===");
        Console.WriteLine("Use ↑/↓ arrows to navigate, SPACE/ENTER to select, ESC to cancel\n");

        if (!availableDirections.Any())
        {
            Console.WriteLine("  No exits available!");
            return;
        }

        for (int i = 0; i < availableDirections.Count; i++)
        {
            Direction dir = availableDirections[i];
            string display = dir.ToDirectionString();

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

    public Direction? GetSelection()
    {
        if (!availableDirections.Any())
        {
            Console.WriteLine("\nNo exits available. Press any key to continue...");
            Console.ReadKey(true);
            return null;
        }

        ConsoleKey keyPressed;
        do
        {
            DisplayMenu();
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            switch (keyPressed)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = selectedIndex > 0 ? selectedIndex - 1 : availableDirections.Count - 1;
                    break;

                case ConsoleKey.DownArrow:
                    selectedIndex = selectedIndex < availableDirections.Count - 1 ? selectedIndex + 1 : 0;
                    break;

                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                    Console.Clear();
                    return availableDirections[selectedIndex];

                case ConsoleKey.Escape:
                    Console.Clear();
                    return null;
            }
        } while (keyPressed is not ConsoleKey.Spacebar and not ConsoleKey.Enter and not ConsoleKey.Escape);

        return null;
    }

    public void ResetSelection() => selectedIndex = 0;
}
