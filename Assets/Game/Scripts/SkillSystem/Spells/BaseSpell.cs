using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseSpell : ScriptableObject, ISpell
    {
        [SerializeField]
        protected float damageMultiplier;
        [SerializeField]
        protected float projectileSpeed;
        [SerializeField]
        protected GameObject prefab;
        [SerializeField]
        protected GameObject VFXPrefab;

        public abstract void Cast(Transform spawnPoint, IReadOnlyCombatData combatData);
    }
}