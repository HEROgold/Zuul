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
    public Inventory getBackpack()
    {
        return backpack;
    }

    public Item Place(string itemName)
    {
        // Een tijdelijke item object
        Item temp = null;

        if(backpack.getItems().ContainsKey(itemName))
        {
            // Item bestaat in dictionary
            // Eerst veilig stellen in temp
            temp = backpack.getItems()[itemName];

            // Uit dictionary van huidige Inventory halen
            backpack.getItems().Remove(itemName);
        }

        // Veiliggestelde item teruggeven.
        return temp;
    }

    // A method that allows you to take damage
    // Can later be inplemented for on a hit taken /Maybe with random int
    public void Damage()
	{
		health -=10;
	}

    // Heals the player but only if u can heal with a amount of hp u choose
    // Else print feedback
    public void Heal(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
            if (health <= 100-hptot)
            {
                health += hptot;
                Console.WriteLine($"You healed! Your health is now: {health}HP");
            }
            else
            {
                Console.WriteLine("You aren't all that injured are you?");
            }
        }
        else
        {
            Console.WriteLine("Please add a valid number...");
        }
	}

    // If the player is low show a message
	// If you are very low show a diffrent message
	public void LowHp()
	{
		if (health <= 40 && health >= 30)
		{
			Console.WriteLine($"U feel hurt.");
		}
		else if(health <= 20)
		{
			Console.WriteLine($"U feel miserable. U should heal!");
		}
	}

    // Shows the players HP
	public void SeeHealth()
    {
        Console.WriteLine($"Your health is: {health}HP");
    }

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}

    public void use(Command command)
    {
        if(!command.HasSecondWord())
        {
            Console.WriteLine("What do u want to use.");
        }

        string itemName = command.SecondWord;
        
    }
}