using UnityEngine;

namespace CosmosDefender
{
    public interface ISpell
    {
        SpellType spellType { get; }

        void Cast(Transform spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData);
    }
}