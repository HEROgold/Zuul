using System.Runtime.InteropServices;

class Player
{
    //fields
    private Inventory backpack;
    public int health;
    // auto property
    public Room CurrentRoom { get; set; }
    // constructor

    // Makes a player with HP and a Inventory
    public Player()
    {
        CurrentRoom = null;
        health = 100;
        backpack = new Inventory(100);
    }
    // Methods

    // Allows u to take a item from a room
    public bool TakeFromChest(string itemName)
    {
        return false;
    }

    // A method that allows you to take damage
    // Can later be inplemented for on a hit taken /Maybe with random int
    public void Damage()
	{
		health -=10;
	}

    // Heals the player but only if u can heal 20 hp or more
    // Else print feedback
    public void Heal()
	{
		if (health <= 80)
		{
			health += 20;
			Console.WriteLine($"You healed! Your health is now: {health}HP");
		}
		else
		{
			Console.WriteLine("You aren't all that injured are you?");
		}
	}

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}
}
