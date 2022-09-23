namespace CosmosDefender
{
    public interface IResourceModifier
    {
        void IncreaseResource(ResourceData data, float amount);
        void DecreaseResource(ResourceData data, float amount);
        bool HasEnoughResource(ResourceData data, float cost);
        void UpdateUI();
        ResourceType GetResourceType();
    }
}