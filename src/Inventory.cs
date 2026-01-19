using System.Collections;

class Inventory
{
    //fields
    private int maxWeight;
    private Dictionary<string, Item> items;

    //constructor
    public Inventory(int maxWeight)
    {
        this.maxWeight = maxWeight;
        this.items = new Dictionary<string, Item>();
    }

    public Dictionary<string, Item> getItems()
    {
        return items;
    }

    //methods

    // allows a item to be placed in a room
    public bool Put(string itemName, Item item)
    {
        Console.WriteLine($"Weight is {item.Weight}");
        if(item.Weight < maxWeight)
        {
            items.Add(item.Description, item);
            return true;
        }
        return false;
    }

    public Item Get(string itemName)
    {
        // Een tijdelijke item object
        Item temp = null;

        if(items.ContainsKey(itemName))
        {
            // Item bestaat in dictionary
            // Eerst veilig stellen in temp
            temp = items[itemName];

            // Uit dictionary van huidige Inventory halen
            items.Remove(itemName);
        }

        // Veiliggestelde item teruggeven.
        return temp;
    }

    public int TotalWeight()
    {
        int total = 0;

        foreach(var (key, item) in items)
        {
            total += item.Weight;
        }

        return total;
    }
    
    public int FreeWeight()
    {
       int  freeWeight = maxWeight - TotalWeight();
       return freeWeight;
    }
}
