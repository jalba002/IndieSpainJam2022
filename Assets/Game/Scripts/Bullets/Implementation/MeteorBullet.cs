using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using Sirenix.OdinInspector;
using UnityEngine;

public class MeteorBullet : Bullet
{
    [SerializeField]
    private BaseProjectile prefab;
    
    [Button("Test Bullet")]
    public override void InstantiateBullet(Transform origin, Vector3 forward, Quaternion rotation)
    {
        base.InstantiateBullet(origin, forward, rotation);

        // This starts a crono that spawns meteorites over time. 
        // TODO get the amount of repetitions and delay from the config.
        CronoScheduler.Instance.ScheduleForRepetitions(
            10, 
            0.5f,
            () => SpawnMeteorsAroundArea(transform.position, 5f, 20f, Vector3.down * 20f)
            );
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