using System;
using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceConfig resourceData;

        public ResourceData goddessResourceData;
        public ResourceType resourceType => resourceData.ResourceType;

        private Animator animator;
        private MaterialModifier materialModifier;

        public Action<float, float> OnResourceUpdated;

        public Action OnActivation;

        private void Awake()
        {
            materialModifier = GetComponent<MaterialModifier>();
            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            goddessResourceData = resourceData.baseResource;
            goddessResourceData.CurrentResource = goddessResourceData.StartingResource;
            UpdateUI();
        }

        public void UpdateUI()
        {
            OnResourceUpdated?.Invoke(goddessResourceData.CurrentResource, goddessResourceData.MaxResource);
            //resourceText.text = "Goddess: " + (int)goddessResourceData.CurrentResource;
        }

        public float GetCurrentResourceAmout()
        {
            return goddessResourceData.CurrentResource;
        }

        public void OnResourceSpent(float cost)
        {
            goddessResourceData.CurrentResource -= cost;
            goddessResourceData.CurrentResource = Mathf.Clamp(goddessResourceData.CurrentResource, 0, goddessResourceData.MaxResource);
            animator.SetTrigger("GoddessMode");
            materialModifier.ChangeMaterial(true);
            CronoScheduler.Instance.ScheduleForTime(10f, () => materialModifier.ChangeMaterial(false));
            OnActivation?.Invoke();
            UpdateUI();
        }

        public void IncreaseResource(float amount)
        {
            goddessResourceData.CurrentResource += amount;
            goddessResourceData.CurrentResource = Mathf.Clamp(goddessResourceData.CurrentResource, 0, goddessResourceData.MaxResource);
            UpdateUI();
        }

        public void IncreaseResourcePerSecond()
        {
            IncreaseResource(goddessResourceData.ResourceOverTime);
        }

        public ResourceData GetResourceData()
        {
            return goddessResourceData;
        }
    }
}