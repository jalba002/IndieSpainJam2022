using UnityEngine;

namespace CosmosDefender
{
    public interface ISpell
    {
        void Cast(Transform spawnPoint, IReadOnlyCombatData combatData);
    }
}