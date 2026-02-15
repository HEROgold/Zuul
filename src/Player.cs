using System;
using System.Collections.Generic;

class Player
{
    private PlayerStats stats;
    private Inventory backpack;
    public int enemyAttack = 0;
    private Random rndnum = new Random();

    public Room CurrentRoom { get; set; }
    public PlayerStats Stats => stats;

    public Player(int initialHealth = 100)
    {
        CurrentRoom = null;
        stats = new PlayerStats(initialHealth);
        backpack = new Inventory(100);
    }

    public Inventory GetBackpack() => backpack;

    public void TakeDamage(int damage) => stats = stats.TakeDamage(damage);
    public void Heal(int amount) => stats = stats.Heal(amount);
    public void FullHeal() => stats = stats.FullHeal();
    public void AddScore(int points) => stats = stats.AddScore(points);
    public void IncrementMoveCounter() => stats = stats.IncrementMoves();

    public bool IsAlive => stats.IsAlive;
    public int Health => stats.Health;
    public int MaxHealth => stats.MaxHealth;
    public int Score => stats.Score;
    public int MovesCount => stats.MovesCount;

    public string GetHealthDisplay() => stats.GetHealthBar();
    public string GetStatusDescription() => stats.GetStatusDescription();
    public string GetFullStatus() => $"{stats}\n{GetStatusDescription()}";

    // Damage method for compatibility with incoming features
    public void Damage()
    {
        TakeDamage(10);
    }

    // Check if health is low and display warnings
    public void LowHp()
    {
        if (Health <= 40 && Health >= 30)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You feel hurt.");
            Console.ForegroundColor = ConsoleColor.White;
        }
        else if (Health <= 20)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"You feel miserable. U should heal!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

    // Check weight of inventory
    public void CheckWeight()
    {
        Console.Write("Total used weight is: ");
        Console.WriteLine(backpack.TotalWeight());
        Console.Write("U have: ");
        Console.WriteLine($"{backpack.FreeWeight()} weight left\n");
    }

    // Enemy attack logic
    public void EnemyAttack()
    {
        int dodgenr = rndnum.Next(1, 11);
        if (dodgenr == 1)
        {
            // Dodged, no damage
            enemyAttack = 0;
        }
        else
        {
            enemyAttack = rndnum.Next(5, 15);
            TakeDamage(enemyAttack);
        }
    }

    // Take item from room chest
    public bool TakeFromChest(string itemName)
    {
        Item testtemp = CurrentRoom.Chest.Peek(itemName);
        if (testtemp != null)
        {
            Item item = CurrentRoom.Chest.Get(itemName);
            backpack.Put(itemName, item);
            Console.WriteLine($"You took {itemName} from the chest.");
            return true;
        }
        Console.WriteLine($"The item {itemName} doesn't exist here.");
        return false;
    }

    // Place item in room chest
    public bool PutInChest(string itemName)
    {
        if (backpack != null)
        {
            Item item = RemoveFromBackpack(itemName);
            if (item != null)
            {
                CurrentRoom.Chest.Put(itemName, item);
                return true;
            }
        }
        return false;
    }

    // Remove item from backpack
    private Item RemoveFromBackpack(string itemName)
    {
        Item temp = null;
        if (backpack.getItems().ContainsKey(itemName))
        {
            temp = backpack.getItems()[itemName];
            backpack.getItems().Remove(itemName);
        }
        return temp;
    }

    // Use an item
    public void UseItem(string itemName, Direction? direction = null)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Console.WriteLine("What do u want to use?\n");
            return;
        }

        Item item = backpack.Peek(itemName);
        if (item == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't have that as a item.\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(CurrentRoom.GetLongDescription());
            return;
        }

        backpack.Get(itemName);

        switch (itemName)
        {
            case "bandage":
                if (Health <= MaxHealth - 20)
                {
                    Heal(20);
                    Console.WriteLine("You used the bandage +20HP");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You healed! Your health is now: {Health}HP\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("I'm not all that hurt, so I have no need for this now.\n");
                }
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "medkit":
                if (Health <= MaxHealth - 50)
                {
                    int healAmount = MaxHealth - Health;
                    Console.WriteLine($"U used the medkit +{healAmount}HP");
                    FullHeal();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You healed! Your health is now: {Health}HP\n");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.WriteLine("It would be a waste to use this right now.\n");
                }
                Console.WriteLine(CurrentRoom.GetLongDescription());
                break;

            case "key":
                if (direction.HasValue)
                {
                    Room lockedRoom = CurrentRoom.GetExit(direction.Value)?.Destination;
                    if (lockedRoom == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("No room in that direction.\n");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(CurrentRoom.GetLongDescription());
                        return;
                    }

                    if (!lockedRoom.GetLock())
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
                }
                else
                {
                    Console.WriteLine("Use key in which direction?\n");
                }
                break;

            default:
                Console.WriteLine($"You can't use {itemName} right now.\n");
                break;
        }
    }

    // Craft items
    public string Craft(string item1, string item2, string item3)
    {
        if (string.IsNullOrEmpty(item1) || string.IsNullOrEmpty(item2))
        {
            Console.WriteLine("What do u want to use in crafting.\n");
            return null;
        }

        if (string.IsNullOrEmpty(item3))
        {
            Console.WriteLine("I need one more thing.\n");
            return null;
        }

        Item craftItem = backpack.Peek(item1);
        Item craftItem2 = backpack.Peek(item2);
        Item craftItem3 = backpack.Peek(item3);

        if (craftItem == null || craftItem2 == null || craftItem3 == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You don't have that item.");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }

        Dictionary<string, Item> craftingItems = new Dictionary<string, Item>();
        craftingItems.Add(item1, craftItem);
        craftingItems.Add(item2, craftItem2);
        craftingItems.Add(item3, craftItem3);

        if (craftingItems.ContainsKey("metalrod") && craftingItems.ContainsKey("piston") && craftingItems.ContainsKey("ducttape"))
        {
            // Remove items from backpack
            backpack.Get(item1);
            backpack.Get(item2);
            backpack.Get(item3);

            Console.WriteLine("You crafted hydraulics!\n");
            backpack.Put("hydraulics", new Item(70, "hydraulics"));
            return "hydraulics";
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You can't craft that.\n");
            Console.ForegroundColor = ConsoleColor.White;
            return null;
        }
    }

    // For testing - instant death
    public void Sepukku()
    {
        stats = stats.TakeDamage(MaxHealth);
    }
}
