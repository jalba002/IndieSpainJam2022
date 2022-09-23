using CosmosDefender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    private Animator animator;
    //private EnemySoundPlayer sounds;
    private ScreenShake screenShake;
    [SerializeField]
    private ResourceData starResourceData;

    [SerializeField]
    private EnemyData data;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        //sounds = GetComponent<EnemySoundPlayer>();
        screenShake = FindObjectOfType<ScreenShake>();
    }

    public override void Start()
    {
        MaxHealth = data.MaxHealth;

        base.Start();
    }

    public override void Die()
    {
        //animator.SetTrigger("Death");
        //sounds.PlayDamageSound();
        GameManager.Instance.StarResourceBehavior.IncreaseResource(starResourceData, data.StarResourceOnDeath);
        GameManager.Instance.GoddessResourceBehavior.IncreaseResource(data.GoddessResourceOnDeath);
        Destroy(gameObject);
    }

    public override void DamageFeedback()
    {
        //animator.SetTrigger("TakeDamage");
        //sounds.PlayDamageSound();
        //screenShake.CameraShake(0.1f, 0.75f);
    }
}
