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

    [SerializeField] protected float bossDamage = 35f;
    [SerializeField] protected float maxDuration = 20f;

    [Header("Deflect Settings")]
    [SerializeField] protected bool canBeDeflected = false;
    [SerializeField] protected float deflectedMultiplier = 2f;

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 
        // Debug.Log($"Has hit {other.gameObject.name}");

        if (other.tag == "Player")
        {
            //other.GetComponent<PlayerHealthManager>().DecreaseHealth(playerDamage);
        }

        if (other.tag == "PlayerShield")
        {
            if(canBeDeflected)
                ReturnProjectile();
        }

        if (other.tag == "Boss" && gameObject.tag == "PlayerDeflectedAttack")
        {
            //other.GetComponentInParent<BossHealthManager>().DecreaseHealth(bossDamage);
            Destroy(this.gameObject);
        }
    }

    public void Shoot(Vector3 vel, bool destroy = true)
    {
        _rigidbody.velocity = vel;
        if(destroy)
            Destroy(this.gameObject, maxDuration);
        //if (destroy)
            //StartCoroutine(DestroyCoroutine());
    }

    void ReturnProjectile()
    {
        _rigidbody.velocity = -_rigidbody.velocity * deflectedMultiplier;
        tag = "PlayerDeflectedAttack";
    }

    IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(maxDuration);
        Destroy(this.gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}