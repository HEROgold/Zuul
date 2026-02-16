public class ItemMenuSystem(IEnumerable<Item> availableItems) : BaseMenuSystem<Item?>(availableItems.Cast<Item?>())
{
    private string customTitle = "Select an Item";

    protected override string GetTitle() => customTitle;

    protected override string FormatOption(Item? item) =>
        item.HasValue ? $"{item.Value.Description} ({item.Value.Weight}kg)" : "None";

    protected override Item? GetCancelValue() => null;

    public Item? GetSelection(string title)
    {
        customTitle = title;
        return GetSelection();
    }
}
