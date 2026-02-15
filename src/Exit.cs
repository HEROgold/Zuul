public record Exit(Room Destination, Action<Player> OnTraverse = null)
{
    public void Traverse(Player player)
    {
        OnTraverse?.Invoke(player);
        player.CurrentRoom = Destination;
    }
}
