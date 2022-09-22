using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : HealthManager
{
    private Animator animator;
    //private EnemySoundPlayer sounds;
    private ScreenShake screenShake;

    private void Awake()
    {
        //animator = GetComponent<Animator>();
        //sounds = GetComponent<EnemySoundPlayer>();
        screenShake = FindObjectOfType<ScreenShake>();
    }

    public override void Die()
    {
        //animator.SetTrigger("Death");
        //sounds.PlayDamageSound();
        Destroy(gameObject);
    }

    public override void DamageFeedback()
    {
        //animator.SetTrigger("TakeDamage");
        //sounds.PlayDamageSound();
        //screenShake.CameraShake(0.1f, 0.75f);
    }
}
