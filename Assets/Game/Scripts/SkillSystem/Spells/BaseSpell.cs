using UnityEngine;

namespace CosmosDefender
{
    public abstract class BaseSpell<T> : ScriptableObject, ISpell where T : MonoBehaviour
    {
        [SerializeField]
        protected float damageMultiplier;
        [SerializeField]
        protected float projectileSpeed;
        [SerializeField]
        protected T prefab;
        [SerializeField]
        protected GameObject VFXPrefab;

        public abstract void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);

    }
}