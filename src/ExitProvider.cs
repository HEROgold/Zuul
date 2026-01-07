using System;

static class ExitProvider
{
    public static Exit Normal(Room destination) => new(destination);

    public static Exit Hazardous(Room destination, int minDamage, int maxDamage) => 
        new(destination, player =>
        {
            int damage = Random.Shared.Next(minDamage, maxDamage + 1);
            player.TakeDamage(damage);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"⚠️  This path is dangerous! You took {damage} damage.");
            Console.ResetColor();
        });

    public static Exit Healing(Room destination, int minHeal, int maxHeal) =>
        new(destination, player =>
        {
            int heal = Random.Shared.Next(minHeal, maxHeal + 1);
            player.Heal(heal);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✚ You passed through the infirmary and healed {heal} health!");
            Console.ResetColor();
        });
}
