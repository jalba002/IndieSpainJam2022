using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseModifier<T, T1> : ScriptableObject, IBuffProvider where T : IModifier<T1>
    {
        [SerializeField]
        private Sprite buffSprite;
        [SerializeField]
        private ModifierPriority priority;

        public Sprite BuffSprite => buffSprite;
        public ModifierPriority Priority => priority;

        public abstract void Modify(ref T1 data);
    }
}