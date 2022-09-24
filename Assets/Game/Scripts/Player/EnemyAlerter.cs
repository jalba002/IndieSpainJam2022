using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class EnemyAlerter : MonoBehaviour
    {
        // Cast over time an area that alerts nearby enemies of your presence.
        [SerializeField] private float radius = 1f;
        
        [SerializeField]
        private float delay = 1f;

        [SerializeField] private LayerMask enemyLayer;

        private float timeToCast;

        private List<EnemyAI> hitEnemies;
        private void Start()
        {
            timeToCast = Time.time + delay;
        }

        private void Update()
        {
            if (Time.time > timeToCast)
            {
                AlertEnemies();
                timeToCast = Time.time + delay;
            }
        }

        private void AlertEnemies()
        {
            // Cast sphere from player.
            // Every enemy collider, set the AI to notice the player.
            // The enemy will forget the player inside their spell.
            var a = AreaAttacksManager.SphereOverlap(transform.position, radius, enemyLayer);
            foreach (var enemy in a)
            {
                enemy.GetComponent<EnemyAI>().Alert(this.gameObject.transform);
            }
        }
    }
}