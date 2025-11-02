public struct Damage
{
    public int Amount;
    public enum Type
    {
        PHYSICAL // maybe energy as well? explosive? idk
    }
    public Type type;
    
    public Damage(int amount, Type type)
    {
        Amount = amount;
        this.type = type;
    }
}