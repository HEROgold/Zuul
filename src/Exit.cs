using System;

class Exit
{
    public Room Destination { get; init; }
    public Action<Player> OnTraverse { get; init; }

    public Exit(Room destination, Action<Player> onTraverse = null) => 
        (Destination, OnTraverse) = (destination, onTraverse);

    public void Traverse(Player player)
    {
        OnTraverse?.Invoke(player);
        player.CurrentRoom = Destination;
    }
}
