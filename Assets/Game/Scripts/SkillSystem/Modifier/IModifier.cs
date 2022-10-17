namespace CosmosDefender
{
    public enum ModifierPriority
    {
        PreModify = 0,
        NormalModify = 1,
        PostModify = 2
    }

    public interface IModifier<T>
    {
        ModifierPriority Priority { get; }

        void Modify(ref T data);
    }

    public interface ISpellModifier : IModifier<SpellData>
    {
        SpellType SpellType { get; }
    }

    public interface IAttributeModifier : IModifier<AttributesData>
    {
        
    }
}