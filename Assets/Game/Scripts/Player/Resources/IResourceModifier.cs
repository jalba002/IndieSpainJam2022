namespace CosmosDefender
{
    public interface IResourceModifier
    {
        ResourceType resourceType { get; }
        void OnResourceSpent(float cost);
        void IncreaseResource(float amount);
        void IncreaseResourcePerSecond();
        void UpdateUI();
        float GetCurrentResourceAmout();
        ResourceData GetResourceData();
    }
}