public partial class CraftingSystem
{
    private class CraftingRecipe(Item[] requiredItems, Item result)
    {
        public Item[] RequiredItems { get; } = requiredItems;
        public Item Result { get; } = result;

        public bool TryCraft(Inventory inventory, Item[] inputItems, out Item? craftedItem)
        {
            craftedItem = null;

            // Validate player has all items
            var missingItems = inputItems.Where(item => inventory.Peek(item.Description) == null).ToList();
            if (missingItems.Count != 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You're missing: {string.Join(", ", missingItems.Select(i => i.Description))}");
                Console.ForegroundColor = ConsoleColor.White;
                return false;
            }

            // Consume ingredients
            foreach (var item in inputItems)
                inventory.Get(item.Description);

            // Add crafted item
            craftedItem = Result;
            inventory.Put(craftedItem.Value);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"You crafted {craftedItem.Value.Description}!");
            Console.ForegroundColor = ConsoleColor.White;
            return true;
        }
    }
}
