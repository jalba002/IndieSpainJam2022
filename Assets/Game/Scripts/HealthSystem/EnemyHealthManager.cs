using System;
using CosmosDefender;
using System.Collections.Generic;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealthManager : HealthManager
{
    private Animator animator;

    private ScreenShake screenShake;

    [Header("Settings")] [SerializeField] protected EnemyData data;

    [SerializeField] protected bool destroyOnDie = true;

    private EnemySpawner enemySpawner;
    private EnemyAI enemyAI;

    [Header("VFX")] [SerializeField] protected VisualEffect prefab;
    [SerializeField] protected List<SkinnedMeshRenderer> attachedMeshRenderers;
    [SerializeField] protected int particleAmountOverride = 400;

    [Header("Sounds")] [Tooltip("Damaged")] [SerializeField]
    protected StudioEventEmitter damageSound;

    [Tooltip("Die")] [SerializeField] protected StudioEventEmitter dieSound;

    private float timeForSoundFeedback = 0f;
    private float timeForAnimationFeedback = 0f;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        screenShake = FindObjectOfType<ScreenShake>();
        enemyAI = GetComponent<EnemyAI>();
    }

    public override void Start()
    {
        MaxHealth = data.MaxHealth;
        base.Start();
    }

    public void Initialize(EnemySpawner spawnPoint)
    {
        this.enemySpawner = spawnPoint;
    }

    [Button]
    public override void Die()
    {
        //animator.SetTrigger("Death");
        //sounds.PlayDamageSound();
        if (GameManager.Instance.ResourceManager != null)
        {
            GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Stars, data.StarResourceOnDeath);
            GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Goddess, data.GoddessResourceOnDeath);
            enemySpawner.DecreaseCurrentEnemyCounter();
        }

        enemyAI.Death();

        dieSound.Play();

        foreach (var meshRenderer in attachedMeshRenderers)
        {
            var vfxObject = Instantiate(prefab, meshRenderer.transform.position,
                meshRenderer.transform.rotation);

            Mesh m = new Mesh
            {
                name = "TemporalSkinnedMesh",
            };

            vfxObject.SetInt("ParticleAmount", particleAmountOverride);
            // TODO REmoved Property Binder
            //vfxObject.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>().Init("VictimTransform", meshRenderer.transform);
            meshRenderer.BakeMesh(m);
            vfxObject.SetMesh("VictimMesh", m);

            Destroy(vfxObject.gameObject, 4f);
        }


        if (destroyOnDie)
            Destroy(gameObject);
    }

    [Button]
    public override void DamageFeedback()
    {
        if (Time.time > timeForSoundFeedback)
        {
            damageSound.Play();
            timeForSoundFeedback = Time.time + .7f;
        }

        if (Time.time > timeForAnimationFeedback)
        {
            animator.SetTrigger("Damaged");
            timeForAnimationFeedback = Time.time + .15f;
        }
    }


}