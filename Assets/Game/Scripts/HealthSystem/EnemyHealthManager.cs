using CosmosDefender;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using FMODUnity;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.VFX.Utility;

public class EnemyHealthManager : HealthManager
{
    private Animator animator;

    private ScreenShake screenShake;

    [Header("Settings")] [SerializeField] private EnemyData data;

    [SerializeField] private bool destroyOnDie = true;

    private EnemySpawner enemySpawner;
    private EnemyAI enemyAI;

    [Header("VFX")] [SerializeField] private Vector3 sizeOverride = new Vector3(5f, 20f, 5f);
    [SerializeField] private VisualEffect prefab;
    [SerializeField] private SkinnedMeshRenderer attachedMRtoRender;

    [Header("Sounds")]
    [Tooltip("Damaged")]
    [SerializeField] private StudioEventEmitter damageSound;
    [Tooltip("Die")]
    [SerializeField] private StudioEventEmitter dieSound;

    private float timeForNextDamageFeedback = 0f;

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

    [Button]
    public override void Die()
    {
        //animator.SetTrigger("Death");
        //sounds.PlayDamageSound();
        if (GameManager.Instance.ResourceManager != null)
        {
            GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Stars, data.StarResourceOnDeath);
            GameManager.Instance.ResourceManager.IncreaseResource(ResourceType.Goddess, data.StarResourceOnDeath);
            enemySpawner.DecreaseCurrentEnemyCounter();
        }

        enemyAI.Death();

        dieSound.Play();

        var vfxObject = Instantiate(prefab, attachedMRtoRender.transform.position,
            attachedMRtoRender.transform.rotation);
        
        vfxObject.SetVector3("Size", sizeOverride);
        Mesh m = new Mesh
        {
            name = "TemporalSkinnedMesh",
        };
        
        vfxObject.gameObject.GetComponent<VFXPropertyBinder>().AddPropertyBinder<VFXTransformBinder>()
            .Init("VictimTransform", attachedMRtoRender.transform);
        attachedMRtoRender.BakeMesh(m);
        vfxObject.SetMesh("VictimMesh", m);

        Destroy(vfxObject.gameObject, 4f);

        if (destroyOnDie)
            Destroy(gameObject);
    }

    [Button]
    public override void DamageFeedback()
    {
        if (Time.time > timeForNextDamageFeedback)
        {
            animator.SetTrigger("Damaged");
            damageSound.Play();
            timeForNextDamageFeedback = Time.time + 0.15f;
        }
    }

    public void SetEnemySpawner(EnemySpawner enemySpawner)
    {
        this.enemySpawner = enemySpawner;
    }
}