public partial class CraftingSystem
{
    private readonly Dictionary<string, CraftingRecipe> recipes = [];

    public CraftingSystem()
    {
        // Register hydraulics recipe
        recipes["hydraulics"] = new(
            [ItemProvider.MetalRod, ItemProvider.Piston, ItemProvider.DuctTape],
            ItemProvider.Hydraulics
        );
    }

    public bool TryCraft(Inventory inventory, Item[] inputItems, out Item? craftedItem)
    {
        craftedItem = null;

        if (inputItems == null || inputItems.Length == 0)
        {
            Console.WriteLine("Need items to craft.");
            return false;
        }

        // Find matching recipe
        var inputSet = new HashSet<Item>(inputItems);
        var recipe = recipes.Values.FirstOrDefault(r =>
            r.RequiredItems.Length == inputItems.Length &&
            inputSet.SetEquals(r.RequiredItems));

        if (recipe == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't craft anything with those items.");
            Console.ForegroundColor = ConsoleColor.White;
            return false;
        }

        return recipe.TryCraft(inventory, inputItems, out craftedItem);
    }
}
