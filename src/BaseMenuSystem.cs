public abstract class BaseMenuSystem<T>(IEnumerable<T> items)
{
    protected readonly List<T> options = [.. items];
    protected int selectedIndex = 0;

    protected abstract string GetTitle();
    protected abstract string FormatOption(T option);
    protected abstract T GetCancelValue();

    protected void DisplayMenu()
    {
        Console.Clear();
        Console.WriteLine($"=== {GetTitle()} ===");
        Console.WriteLine("Use ↑/↓ arrows to navigate, SPACE/ENTER to select, ESC to cancel\n");

        if (options.Count == 0)
        {
            Console.WriteLine("No options available.");
            return;
        }

        for (int i = 0; i < options.Count; i++)
        {
            string display = FormatOption(options[i]);

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

    public T GetSelection()
    {
        if (options.Count == 0)
        {
            Console.WriteLine("No options available.");
            Console.ReadKey(true);
            Console.Clear();
            return GetCancelValue();
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
                    return GetCancelValue();
            }
        } while (keyPressed is not ConsoleKey.Spacebar and not ConsoleKey.Enter and not ConsoleKey.Escape);

        return GetCancelValue();
    }

    public void ResetSelection() => selectedIndex = 0;
}
