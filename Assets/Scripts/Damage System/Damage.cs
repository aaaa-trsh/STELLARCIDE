/// <summary>
/// Struct storing a damage amount and type for future uses
/// such as resistance and damage bonuses
/// </summary>
public struct Damage
{
    public int Amount;
    public enum Type
    {
        PHYSICAL // maybe energy as well? explosive? idk
    }
    public Type type;


    /// <param name="amount"> Integer describing the amount of damage something will take [-inf, inf]</param>
    /// <param name="type"> Can be from {Type.PHYSICAL, ...} can be further expanded in Damage.cs</param>
    public Damage(int amount, Type type)
    {
        Amount = amount;
        this.type = type;
    }
}