namespace CosmosDefender
{
    public interface IResourceModifier
    {
        void IncreaseResource(float value);
        void DecreaseResource(float value);
        void CanUseResource(float value);
        ResourceType GetResourceType();
    }
}