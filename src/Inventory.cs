public class Inventory(int maxWeight)
{
    public Dictionary<string, Item> Items { get; } = [];
    public int MaxWeight { get; private set; } = maxWeight;
    public int TotalWeight() => Items.Values.Sum(item => item.Weight);
    public int FreeWeight() => MaxWeight - TotalWeight();

    public void IncreaseMaxWeight(int amount)
    {
        MaxWeight += amount;
    }

    public bool Put(Item item)
    {
        if (item.Weight > FreeWeight())
        {
            Console.WriteLine($"Can't carry anymore\nWeight left: {FreeWeight()}, item weight: {item.Weight}");
            return false;
        }
        Items.Add(item.Description, item);
        return true;
    }

    public Item? Get(string itemName)
    {
        if (!Items.TryGetValue(itemName, out var item)) return null;
        Items.Remove(itemName);
        return item;
    }

    public Item? Peek(string itemName) => Items.TryGetValue(itemName, out var item) ? item : null;

    public void Display()
    {
        Console.WriteLine("=== Backpack ===");
        foreach (var item in Items.Values)
            Console.WriteLine($"{item.Description} - {item.Weight}kg");
        Console.WriteLine($"Weight: {TotalWeight()}/{MaxWeight} ({FreeWeight()} remaining)");
    }
}
