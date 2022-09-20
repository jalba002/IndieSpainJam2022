using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PlayerAttributes), menuName = "CosmosDefender/" + nameof(PlayerAttributes))]
    public class PlayerAttributes : ScriptableObject, IReadOnlyCombatData, IReadOnlySpeedData
    {
        [SerializeField]
        private AttributesData attributes;

        [SerializeField]
        private List<BaseAttributeModifier> attributeModifiers;

        private AttributesData currentAttributes;

        [ShowInInspector]
        public float AttackDamage => currentAttributes.AttackDamage;
        [ShowInInspector]
        public float Speed => currentAttributes.Speed;

        private void UpdateAttributes()
        {
            currentAttributes = attributes;
            foreach (var modifier in attributeModifiers.OrderBy(x => x.Priority))
                modifier.Modify(ref currentAttributes);
        }

        public void AddAttribute(BaseAttributeModifier attribute)
        {
            attributeModifiers.Add(attribute);
            UpdateAttributes();
        }

        public void AddAttributes(List<BaseAttributeModifier> attributes)
        {
            attributeModifiers.AddRange(attributes);
            UpdateAttributes();
        }

        [Button]
        public void AddTemporalAttribute(BaseTemporalAttributeModifier attribute)
        {
            AddAttribute(attribute);
            CronoScheduler.Instance.ScheduleForTime(attribute.Time, () => RemoveAttribute(attribute));
        }

        public void RemoveAttribute(BaseAttributeModifier attribute)
        {
            attributeModifiers.Remove(attribute);
            UpdateAttributes();
        }

        public void RemoveAllModifiers()
        {
            attributeModifiers.Clear();
            UpdateAttributes();
        }
    }
}