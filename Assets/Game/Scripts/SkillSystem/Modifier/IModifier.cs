namespace CosmosDefender
{
    public enum ModifierPriority
    {
        PreModify = 0,
        NormalModify = 1,
        PostModify = 2
    }

    public interface ISpellModifier<T>
    {
        ModifierPriority Priority { get; }

        void Modify(ref T data);
    }

    public interface ISpellModifier : ISpellModifier<SpellData>
    {
        SpellType SpellType { get; }
    }
}