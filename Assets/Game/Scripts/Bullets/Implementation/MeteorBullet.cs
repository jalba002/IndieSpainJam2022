using UnityEngine;

namespace CosmosDefender.Bullets.Implementation
{
    public class MeteorBullet : BaseBullet
    {
        [SerializeField] private BaseProjectile prefab;

        //[Button("Test Bullet")]
        public override void InstantiateBullet(Transform origin, Vector3 forward, Quaternion rotation,
            IReadOnlyOffensiveData combatData, SpellData spellData)
        {
            base.InstantiateBullet(origin, forward, rotation, combatData, spellData);
            // This starts a crono that spawns meteorites over time. 
            // TODO get the amount of repetitions and delay from the config.
            CronoScheduler.Instance.ScheduleForRepetitions(
                (int) spellData.Amount,
                spellData.ProjectileDelay,
                () => SpawnMeteorsAroundArea(transform.position, spellData.UniformSize * 0.5f, 20f,
                    Vector3.down * spellData.Speed)
            );
            CronoScheduler.Instance.ScheduleForTime(spellData.Amount * spellData.ProjectileDelay + 1f, () =>
            {
                Destroy(this.gameObject);
            });
        }

        private void SpawnMeteorsAroundArea(Vector3 origin, float radius, float height, Vector3 speed)
        {
            // Use the area to determine where the new meteorite will fall.
            // 
            Vector2 modOrigin = origin;
            modOrigin += Random.insideUnitCircle * radius;
            Vector3 spawnPos = new Vector3(modOrigin.x, origin.y + height, modOrigin.y);

            var instance = Instantiate(prefab, spawnPos, Quaternion.identity);
            instance.InitializeProjectile(spawnPos, speed);
        }
    }
}