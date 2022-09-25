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
    private ResourceConfig starResourceData;

    [SerializeField]
    private EnemyData data;

    private EnemySpawner enemySpawner;
    private EnemyAI enemyAI;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        //sounds = GetComponent<EnemySoundPlayer>();
        screenShake = FindObjectOfType<ScreenShake>();
        enemyAI = GetComponent<EnemyAI>();
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
        GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Stars, data.StarResourceOnDeath);
        GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Goddess, data.StarResourceOnDeath);
        enemySpawner.DecreaseCurrentEnemyCounter();
        enemyAI.Death();
        Destroy(gameObject);
    }

    public override void DamageFeedback()
    {
        //animator.SetTrigger("TakeDamage");
        //sounds.PlayDamageSound();
        //screenShake.CameraShake(0.1f, 0.75f);
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }
}
