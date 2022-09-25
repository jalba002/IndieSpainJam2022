using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseTemporalAttributeModifier : BaseAttributeModifier, ITemporalModifier
    {
        [SerializeField]
        private float time;

        public float Time => time;
    }
}