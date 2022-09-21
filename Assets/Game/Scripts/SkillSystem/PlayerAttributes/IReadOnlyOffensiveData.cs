namespace CosmosDefender
{
    public interface IReadOnlyOffensiveData
    {
        float AttackDamage { get; }
        float CooldownReduction { get; }
    }
}