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
        protected Bullet prefab;
        [SerializeField]
        protected GameObject VFXPrefab;

        public abstract void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);
    }
}