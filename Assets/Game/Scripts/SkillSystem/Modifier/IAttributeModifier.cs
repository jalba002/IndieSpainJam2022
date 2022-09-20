namespace CosmosDefender
{
    public enum ModifierPriority
    {
        PreModify = 0,
        NormalModify = 1,
        PostModify = 2
    }

    public interface IAttributeModifier
    {
        ModifierPriority Priority { get; }

        void Modify(ref AttributesData data);
    }
}