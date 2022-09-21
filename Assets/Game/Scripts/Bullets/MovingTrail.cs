using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class MovingTrail : Bullet
{
    public float bulletSpeed = 1f;
    private Rigidbody rb;
    public override void InstantiateBullet()
    {
        base.InstantiateBullet();
        // Setup and coroutine.
        // Get VFX duration.
        // Get component damage.
        // Calculate total.

        VisualEffect vfx = GetComponentInChildren<VisualEffect>();
        rb = GetComponent<Rigidbody>();
        // Get from prefab or VFX.
        float trailDuration = vfx.GetFloat("Duration");
        float groundDuration = vfx.GetFloat("MaxDecalDuration");

        rb.velocity = transform.forward * bulletSpeed;
        
        StartCoroutine(DelayedMovingTrail(trailDuration, groundDuration));
    }

    private void Update()
    {
        CheckOutOfGround();
        SnapToGround();
    }

    private void OnTriggerEnter(Collider other)
    {
        //
        Debug.Log($"{this.name} collided with {other.gameObject.name}");
    }

    IEnumerator DelayedMovingTrail(float delay, float destroyDelay, bool destroy = true)
    {
        //AreaAttacksManager.SphereOverlap(transform.position, 5f);
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        //this.gameObject.SetActive(false);
        yield return new WaitForSeconds(destroyDelay + .5f);
        if (destroy)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
        // Deactivate if object pooler.
    }
}
