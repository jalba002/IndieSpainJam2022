using CosmosDefender;
using UnityEngine;

public class Laser : Bullet
{
    // This laser starts in a position
    // Then casts a large box periodically, every 0.25 secs.
    // That uses the External Coroutine manager with an action.
    // CronoScheduler.Instance.ScheduleForTime();

    public override void InstantiateBullet(Vector3 origin, Vector3 forward, Quaternion rotation)
    {
        this.transform.forward = forward;
        this.transform.rotation = rotation;
        // TODO Gather length size and send here.
        CronoScheduler.Instance.ScheduleForTimeAndExecuteElapsed(3f, 
            0.25f, 
            () => CastRayDamage(origin, new Vector3(1f, 1f, 20f), rotation));
    }
    
    private void CastRayDamage(Vector3 origin, Vector3 length, Quaternion rotation)
    {
        // TODO Gather information about the ray size here.
        AreaAttacksManager.BoxAttack(this.transform.position, length, rotation);
    }
}