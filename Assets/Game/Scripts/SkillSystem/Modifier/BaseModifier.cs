using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseModifier<T, T1> : ScriptableObject, IBuffProvider where T : IModifier<T1>
    {
        [Header("Data")]
        [SerializeField] [Tooltip("El tier determina cuantos + se muestran en el icono.")]
        [Range(0, 2)]
        private int tier = 0;
        [SerializeField]
        private ModifierPriority priority;
        
        [Header("Beauty")]
        [SerializeField]
        private Sprite buffSprite;

        public Sprite BuffSprite => buffSprite;
        public ModifierPriority Priority => priority;
        public int Tier => tier;

        public abstract void Modify(ref T1 data);
        public abstract float GetInitialValue(T1 data);
        public abstract float GetFinalValue(T1 data);
    }
}