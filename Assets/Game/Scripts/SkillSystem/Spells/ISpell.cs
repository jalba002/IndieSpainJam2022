using UnityEngine;

namespace CosmosDefender
{
    public interface ISpell
    {
        SpellType spellType { get; }

        void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation);
        void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, SpellManager caster);
    }
}