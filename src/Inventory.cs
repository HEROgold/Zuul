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

    //methods
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
        if(items.ContainsKey(itemName))
        {
            items.Remove(itemName);
        }
        return null;
    }

    public int TotalWeight()
    {
        int total = 0;
        foreach(KeyValuePair<string, Item> item in items)
        {
            //Curently not working
            //total += item.Weight;
        }
        return total;
    }
    
    public int FreeWeight()
    {
       int  freeWeight = maxWeight - TotalWeight();
       return freeWeight;
    }
}
