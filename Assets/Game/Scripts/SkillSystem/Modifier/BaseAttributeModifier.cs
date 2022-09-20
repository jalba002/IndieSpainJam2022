using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseAttributeModifier : ScriptableObject, IAttributeModifier
    {
        [SerializeField]
        private ModifierPriority priority;

        public ModifierPriority Priority => priority;

        public abstract void Modify(ref AttributesData data);
    }
}