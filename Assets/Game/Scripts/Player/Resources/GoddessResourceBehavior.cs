using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class GoddessResourceBehavior : MonoBehaviour, IResourceModifier
    {
        [SerializeField] private ResourceConfig resourceData;
        [SerializeField] private TextMeshProUGUI resourceText;

        public ResourceData goddessResourceData;
        public ResourceType resourceType => resourceData.ResourceType;

        private Animator animator;
        private MaterialModifier materialModifier;

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
            resourceText.text = "Goddess: " + (int)goddessResourceData.CurrentResource;
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