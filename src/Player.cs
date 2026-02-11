using System.Runtime.InteropServices;

class Player
{
    //fields
    private Inventory backpack;
    public int health;
    // auto property
    public Room CurrentRoom { get; set; }
    public int enemyAttack = 0;
    Random rndnum = new Random();
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
        // A temp item object
        Item temp = null;

        if(backpack.getItems().ContainsKey(itemName))
        {
            // Item exists in Inventory?
            // Secure item
            temp = backpack.getItems()[itemName];

            // Remove from dictionary
            backpack.getItems().Remove(itemName);
        }

        // Return saved item
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
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You healed! Your health is now: {health}HP");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("You aren't all that injured are you?");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Please add a valid number...");
            Console.ForegroundColor = ConsoleColor.White;
        }
	}

    public void damageNum(string hptotstr)
	{
        if (hptotstr != null)
        {
            int hptot=Int32.Parse(hptotstr);
            if (health >= hptot)
            {
                health -= hptot;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You took {hptot} damage! Your health is now: {health}HP");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine("That would kill u.");
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
            Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"You feel hurt.");
            Console.ForegroundColor = ConsoleColor.White;
		}
		else if(health <= 20)
		{
            Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"You feel miserable. U should heal!");
            Console.ForegroundColor = ConsoleColor.White;
		}
	}

    // Shows the players HP
	public void SeeHealth()
    {
        Console.WriteLine($"Your health is: {health}HP\n");
    }
    
    // Allows the player to see how much they are carrying and the space left
	public void checkWeight()
	{
		Console.Write("Total used weight is: ");
		Console.WriteLine(getBackpack().TotalWeight());
		Console.Write("U have: ");
		Console.WriteLine($"{getBackpack().FreeWeight()} weight left\n");
	}

    // Kill urself usefull to test on death triggers
    public void Sepuccu()
	{
		health = 0;
	}

    public void EnemyAttack()
    {
		int dodgenr = rndnum.Next(1,11);
		if (dodgenr == 1)
		{
			health -= enemyAttack;
		}
		else
		{
			enemyAttack = rndnum.Next(5,15);
			health -= enemyAttack;
		}
    }
    // Allows u to take a item from a room
    public bool TakeFromChest(string itemName)
    {
		Item testtemp = CurrentRoom.Chest.Peek(itemName);
        if (testtemp != null)
        {
            // Remove itemName from chest and save it
            Item item = CurrentRoom.Chest.Get(itemName);

            // Add the item we took to the backpack
            getBackpack().Put(itemName, item);

            Console.WriteLine($"You took {itemName} from the chest.");
            // Return true
            return true;
        }
        // If empty return false
        Console.WriteLine($"The item {itemName} doesn't exist here.");
        return false;
    }

    	// Allows you to add items to a room
	public bool PutInChest(string itemName)
    {
        if (getBackpack() != null)
        {
            // Remove itemName from backpack and save it
            Item item = Place(itemName);

            // Add the item we took to the inventory
            CurrentRoom.Chest.Put(itemName, item);

            // Return true
            return true;
        }

        // If empty return false
        return false;
    }
    // Uses a item
    public void Use(Command command)
    {
        if(!command.HasSecondWord())
        {
            Console.WriteLine("What do u want to use?\n");
            return;
        }

        string itemName = command.SecondWord;
        Item item = backpack.Peek(itemName);
        backpack.Get(itemName);
        if (item == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't have that as a item.\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(CurrentRoom.GetLongDescription());
            return;
        }
        
        // Checks what item it is then uses it
        switch(itemName)
        {
            case "bandage":
                if (health <= 100-20)
                {
                    health += 20;
                    Console.WriteLine("You used the bandage +20HP");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You healed! Your health is now: {health}HP\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    Console.WriteLine("I'm not all that hurt, so I have no need for this now.\n");
                }
                
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "medkit":
                if (health <= 100-50)
                {
                    Console.WriteLine($"U used the medkit +{100-health}HP");
                    health = 100;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You healed! Your health is now: {health}HP\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }

                else
                {
                    Console.WriteLine("It would be a waste to use this right now.\n");
                }

                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "key":

                Room lockedRoom = CurrentRoom.GetExit(command.ThirdWord);

                if (lockedRoom == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No room in that direction.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                else if (!lockedRoom.GetLock())
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Room isn't locked.\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(CurrentRoom.GetLongDescription());
                    return;
                }

                lockedRoom.RemoveLock();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("The door is now unlocked.\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;
        }
    }

    // Allows the crafting of the hydrolics item if the correct items are called
    public string Craft(Command command)
    {
        if  (!command.HasSecondWord() || !command.HasThirdWord())
        {
            Console.WriteLine("What do u want to use in crafting.\n");
            return null;
        }

        else if (!command.HasFourthWord())
        {
            Console.WriteLine("I need one more thing.\n");
            return null;
        }

        Item craftItem = backpack.Peek(command.SecondWord);
        Item craftItem2 = backpack.Peek(command.ThirdWord);
        Item craftItem3 = backpack.Peek(command.FourthWord);

        if (craftItem == null || craftItem2 == null || craftItem3 == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't have that item.");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        Dictionary<string,Item> craftingshit = new Dictionary<string, Item>();

        craftingshit.Add(command.SecondWord,craftItem);
        craftingshit.Add(command.ThirdWord,craftItem2);
        craftingshit.Add(command.FourthWord,craftItem3);

        if (!craftingshit.ContainsKey("metalrod") || !craftingshit.ContainsKey("piston") || !craftingshit.ContainsKey("ducttape"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Those weren't the correct items.\n");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        else
        {
            return "hydraulics";
        }
    }
}
