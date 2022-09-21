namespace CosmosDefender
{
    public interface IReadOnlyDefensiveData
    {
        float MaxHealth { get; }
        float HealthRegeneration { get; }
    }
}