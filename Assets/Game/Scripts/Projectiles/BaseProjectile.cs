using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected Collider m_Collider;

    [SerializeField] protected Rigidbody m_Rigidbody;

    public virtual void InitializeProjectile(Vector3 spawnPoint, Vector3 velocity)
    {
        // A.
    }

    protected virtual void UpdateRenderer()
    {
    }

    protected virtual void UpdateRigidbody()
    {
    }

    protected virtual void UpdateParticles()
    {
    }

    protected virtual void CastDamage()
    {
    }

    protected virtual void FinishObject()
    {
    }

    protected virtual void UpdateVFX()
    {
    }
}