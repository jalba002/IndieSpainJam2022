using CosmosDefender;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour, IDamageable
{
    public float MaxHealth = 100f;
    protected float currentHealth;

    protected bool isInvulnerable = false;
    protected Coroutine invulnerableCoroutine;

    public virtual void Start()
    {
        currentHealth = MaxHealth;
    }
    
    public void IncreaseHealth(float value)
    {
        currentHealth += value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);
    }
    [Button]
    public void DecreaseHealth10()
    {
        TakeDamage(10f);
    }

    public void TakeDamage(float value)
    {
        if (isInvulnerable)
            return;

        currentHealth -= value;
        currentHealth = Mathf.Clamp(currentHealth, 0f, MaxHealth);

        if (currentHealth == 0)
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

    public virtual void Die() { }

    public virtual void DamageFeedback() { }
}