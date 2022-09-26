using System;
using CosmosDefender;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    public float MaxHealth = 100f;
    [SerializeField] protected float currentHealth;
    protected bool isDead = false;

    protected bool isInvulnerable = false;
    protected Coroutine invulnerableCoroutine;

    public Action<float, float> OnDamageTaken;

    public virtual void Start()
    {
        currentHealth = MaxHealth;
    }

    public void IncreaseHealth(float value)
    {
        if (isDead)
            return;
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);
    }

    public void TakeDamage(float value)
    {
        if (isInvulnerable || isDead)
            return;

        currentHealth -= value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            DamageFeedback();
        }
    }

    public void SetInvulnerableState(bool newState)
    {
        isInvulnerable = newState;
    }

    public void InvulnerableOverTime(float duration)
    {
        if (isInvulnerable) return;

        if (invulnerableCoroutine != null)
        {
            StopCoroutine(invulnerableCoroutine);
        }

        invulnerableCoroutine = StartCoroutine(InvulerableCo(duration));
    }

    IEnumerator InvulerableCo(float duration)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        isInvulnerable = false;
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual void DamageFeedback()
    {
        OnDamageTaken?.Invoke(currentHealth, MaxHealth);
    }
}