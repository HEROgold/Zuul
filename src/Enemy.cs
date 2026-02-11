class Enemy
{
    public int HealthEnemy;
    public int CurrentHealthEnemy;
    private bool Alive
    {
        get { return CurrentHealthEnemy > 0; }
    }
    public int AttackDamage;
    private string DescriptionEnemy;
    public Room currentroom;

    public Enemy(int maxhealth, string desc, int attackdamage, Room room = null)
    {
        HealthEnemy = maxhealth;
        CurrentHealthEnemy = HealthEnemy;
        AttackDamage = attackdamage;
        DescriptionEnemy = desc;
        currentroom = room;
    }
    public void DamageEnemy(int damage)
    {
        CurrentHealthEnemy -= damage;
    }
    public bool EnemyIsAlive()
    {
        return Alive;
    }
}
