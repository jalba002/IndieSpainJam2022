using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private SphereCollider _collider;
    private Rigidbody _rigidbody;

    [Header("Projectile settings")] [SerializeField]
    protected float playerDamage = 20f;

    [SerializeField] protected float maxDuration = 20f;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
       
    }

    public void ShootProjectile(Vector3 velocity, bool destroy = true)
    {
        _rigidbody.velocity = velocity;
    }
    
    
    // IEnumerator DestroyCoroutine()
    // {
    //     yield return new WaitForSeconds(maxDuration);
    //     Destroy(this.gameObject);
    // }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}