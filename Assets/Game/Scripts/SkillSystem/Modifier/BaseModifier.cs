using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseModifier<T, T1> : ScriptableObject where T : IModifier<T1>
    {
        [SerializeField]
        private ModifierPriority priority;

        public ModifierPriority Priority => priority;

        public abstract void Modify(ref T1 data);
    }
}