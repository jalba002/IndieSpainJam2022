using UnityEngine;
using CosmosDefender.Projectiles;

namespace CosmosDefender.Bullets.Implementation
{
    public class MeteorBullet : BaseBullet
    {
        [SerializeField] private BaseProjectile prefab;

        //[Button("Test Bullet")]
        public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData);

            // This starts a crono that spawns meteorites over time. 
            CronoScheduler.Instance.ScheduleForRepetitions(
                (int) spellData.Amount,
                spellData.ProjectileDelay,
                () => SpawnMeteorsAroundArea(
                    transform.position,
                    20f,
                    combatData,
                    spellData)
            );
            
            CronoScheduler.Instance.ScheduleForTime(spellData.Amount * spellData.ProjectileDelay + 1f, () =>
            {
                Destroy(this.gameObject);
            });
        }

        private void SpawnMeteorsAroundArea(Vector3 origin, float height, IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            // Use the area to determine where the new meteorite will fall.
            // 
            Vector2 modOrigin = new Vector2(origin.x, origin.z);
            modOrigin += Random.insideUnitCircle * spellData.UniformSize;
            Vector3 spawnPos = new Vector3(modOrigin.x, origin.y + height, modOrigin.y);
            var instance = Instantiate(prefab, spawnPos, Quaternion.identity);
            instance.InitializeProjectile(spawnPos, combatData, spellData);
        }
    }
}