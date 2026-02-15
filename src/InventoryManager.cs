class InventoryManager
{
    public static bool Transfer(Inventory source, Inventory destination, string itemName)
    {
        if (source.Peek(itemName) is not Item item)
        {
            Console.WriteLine($"The item '{itemName}' doesn't exist in the source inventory.");
            return false;
        }

        source.Get(itemName);
        if (!destination.Put(item))
        {
            source.Put(item); // Put it back if destination can't accept it
            return false;
        }

        return true;
    }

    public static bool TransferFromChest(Player player, Item? item)
    {
        if (player.CurrentRoom?.Chest is not Inventory chest)
        {
            Console.WriteLine("There's no chest here.");
            return false;
        }

        if (!item.HasValue)
        {
            Console.WriteLine("No item selected.");
            return false;
        }

        if (Transfer(chest, player.Backpack, item.Value.Description))
        {
            Console.WriteLine($"You took {item.Value.Description} from the chest.");
            return true;
        }
        return false;
    }

    public static bool TransferToChest(Player player, Item? item)
    {
        if (player.CurrentRoom?.Chest is not Inventory chest)
        {
            Console.WriteLine("There's no chest here.");
            return false;
        }

        if (!item.HasValue)
        {
            Console.WriteLine("No item selected.");
            return false;
        }

        return Transfer(player.Backpack, chest, item.Value.Description);
    }

    public static bool DropItem(Player player, Item? item)
    {
        if (!item.HasValue)
        {
            Console.WriteLine("No item selected.");
            return false;
        }

        if (player.Backpack.Get(item.Value.Description) is not Item droppedItem)
        {
            Console.WriteLine($"You don't have '{item.Value.Description}'.");
            return false;
        }

        Console.WriteLine($"You dropped {item.Value.Description}.");
        return true;
    }
}
