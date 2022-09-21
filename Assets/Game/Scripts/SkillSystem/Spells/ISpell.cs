using UnityEngine;

namespace CosmosDefender
{
    public interface ISpell
    {
        void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);
    }
}