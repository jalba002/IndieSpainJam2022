using System;
using UnityEngine;

namespace CosmosDefender
{
    [Serializable]
    public struct SpellData
    {
        [Header("Base")]
        [SerializeField]
        private SpellType spellType;
        [SerializeField]
        private float damageMultiplier;
        [SerializeField]
        private float cooldown;
        [SerializeField]
        private float speed;
        [Header("Casting Area")]
        [SerializeField]
        private float uniformSize;
        [Header("Projectile")]
        [SerializeField]
        private int amount;
        [SerializeField] 
        private float projectileRadius;
        [SerializeField] 
        private float projectileDelay;
        [Header("Raycast")]
        [SerializeField]
        private float maxAttackDistance;
        [SerializeField]
        private LayerMask layerMask;
        [Header("VFX")]
        [SerializeField]
        private float lifetime;
        [SerializeField]
        private float activeDuration;
        [Header("Animation")]
        [SerializeField] 
        private string animationCode;
        [SerializeField] 
        private float animationDelay;
        [Header("Preview")]
        [SerializeField] 
        private bool usesPreview;

        public SpellType SpellType => spellType;
        public float DamageMultiplier { get => damageMultiplier; set => damageMultiplier = value; }
        public float Cooldown { get => cooldown; set => cooldown = value; }
        public int Amount { get => amount; set => amount = value; }
        public float Speed { get => speed; set => speed = value; }
        public float UniformSize { get => uniformSize; set => uniformSize = value; }
        public float ProjectileRadius { get => projectileRadius; set => projectileRadius = value; }
        public float ProjectileDelay { get => projectileDelay; set => projectileDelay = value; }
        public float MaxAttackDistance { get => maxAttackDistance; set => maxAttackDistance = value; }
        public float Lifetime { get => lifetime; set => lifetime = value; }
        public float ActiveDuration { get => activeDuration; set => activeDuration = value; }
        public LayerMask LayerMask { get => layerMask; set => layerMask = value; }
        public string AnimationCode { get => animationCode; set => animationCode = value; }
        public float AnimationDelay { get => animationDelay; set => animationDelay = value; }
        public bool UsesPreview { get => usesPreview;  set => usesPreview = value; }
    }
}