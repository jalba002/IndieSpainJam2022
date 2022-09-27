using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseModifier<T, T1> : ScriptableObject, IBuffProvider where T : IModifier<T1>
    {
        [SerializeField]
        private Sprite buffSprite;
        [SerializeField]
        private ModifierPriority priority;

        [SerializeField] [Tooltip("El tier determina cuantos + se muestran en el icono.")]
        [Range(0, 2)]
        private int tier = 0;

        public Sprite BuffSprite => buffSprite;
        public ModifierPriority Priority => priority;
        public int Tier => tier;

        public abstract void Modify(ref T1 data);
    }
}