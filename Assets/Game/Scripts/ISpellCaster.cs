using UnityEngine;

namespace CosmosDefender
{
    public interface ISpellCaster
    {
        GameObject GameObject { get; }

        Animator Animator { get; }
        
        Transform CastingPoint { get; }
        
        void SetAnimationTrigger(string triggerName);
    }
}