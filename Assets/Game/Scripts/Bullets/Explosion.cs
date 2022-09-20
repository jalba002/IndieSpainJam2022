using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class Explosion : Bullet
{
    public float bulletDuration = 5f;
    public override void InstantiateBullet()
    {
        base.InstantiateBullet();
        // Setup and coroutine.
        // Get VFX duration.
        // Get component damage.
        // Calculate total.

        VisualEffect vfx = GetComponent<VisualEffect>();
        
        // Get from prefab or VFX.
        bulletDuration = vfx.GetFloat("Duration");
    }

    private void Update()
    {
        CheckOutOfGround();
        SnapToGround();
    }

    IEnumerator InstantExplosion()
    {
        AreaAttacksManager.SphereOverlap(transform.position, 5f);
        yield return new WaitForSeconds(bulletDuration);
        this.gameObject.SetActive(false);
    }
}
