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
        
        public abstract void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyCombatData combatData);

        //public MissileData CreateMissileData(ICombatData data) => null;
    }
}