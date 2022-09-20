using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseTemporalAttributeModifier : BaseAttributeModifier
    {
        [SerializeField]
        private float time;

        public float Time => time;
    }
}