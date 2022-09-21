using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseModifier<T> : ScriptableObject, IModifier<T>
    {
        [SerializeField]
        private ModifierPriority priority;

        public ModifierPriority Priority => priority;

        public abstract void Modify(ref T data);
    }
}