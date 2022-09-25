using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(TornadoSpell), menuName = "CosmosDefender/" + nameof(TornadoSpell))]
    public class TornadoSpell : BaseSpell
    {
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData, SpellManager caster)
        {
            var instance = Instantiate(prefab, spawnPoint, rotation);
            instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}