using UnityEngine;
using UnityEngine.VFX;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(ArcSpellWeak), menuName = "CosmosDefender/" + nameof(ArcSpellWeak))]
    public class ArcSpellWeak : BaseSpell
    {
        public VisualEffect vfxPrefab;
        protected override void Cast(Vector3 spawnPoint, Vector3 forward, Quaternion rotation, IReadOnlyOffensiveData combatData)
        {
            var spellTesterFirePointPosition = FindObjectOfType<SpellTester>().FirePoint.position;
            Vector3 sp = spawnPoint - (spawnPoint - spellTesterFirePointPosition) /2;
            var vfxItem = Instantiate(vfxPrefab, sp, Quaternion.identity);
            vfxItem.SetVector3("Start", spellTesterFirePointPosition);
            vfxItem.SetVector3("End", spawnPoint);

            Destroy(vfxItem.gameObject, vfxItem.GetFloat("Lifetime"));

            var instance = Instantiate(prefab, spawnPoint, rotation);
            instance.InstantiateBullet(spawnPoint, forward, rotation, combatData, currentData);
        }
    }
}