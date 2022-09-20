using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The bullet work is to instantiate the projectile using a valid position.
public class Bullet : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] protected LayerMask raycastHitLayers;

    [SerializeField] protected float raycastDistance = 10f;
    
    [Header("Damage Settings")]
    [SerializeField] protected int damage = 10;
    
    [Header("Prefabs")]
    [SerializeField] protected bool spawnPrefab = false;
    [SerializeField] protected GameObject projectilePrefab;
    
    [SerializeField]
    protected bool drawSphere = false;

    [SerializeField]
    protected bool IsStopped = false;

    //private PlayerHealthManager playerHealth;

    public virtual void InstantiateBullet()
    {
        // Nah.
    }

    // void Start()
    // {
    //     playerHealth = FindObjectOfType<PlayerHealthManager>();
    // }

    protected virtual void CheckOutOfGround()
    {
        if (IsStopped) return;
    
        RaycastHit hit;
    
        Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        if (Physics.Raycast(distance, transform.TransformDirection(-Vector3.up), out hit, raycastDistance, raycastHitLayers))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
        else
        {
            //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            IsStopped = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    protected virtual void SnapToGround()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(-Vector3.up),  out RaycastHit hit, raycastDistance, raycastHitLayers))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    protected virtual void DealDamageArea(Vector3 center, float diameter)
    {
        var hitTargets = Physics.OverlapSphere(center, diameter * 0.5f);

        foreach (var item in hitTargets)
        {
            if (item.tag == "Player")
            {
                //item.GetComponent<PlayerHealthManager>().DecreaseHealth(damage);
                Destroy(this.gameObject);
                break;
            }
        }
    }
}
