using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class PillarAttributeModifierBehavior : MonoBehaviour, IPillarObserverModifier
    {
        [SerializeField]
        private List<BaseAttributeModifier> attributeModifier = new List<BaseAttributeModifier>();

        [SerializeField]
        private List<BaseTemporalAttributeModifier> empoweredAttributeModifier = new List<BaseTemporalAttributeModifier>();

        public void OnObserverInRange(PillarObserver observer)
        {
            observer.AddModifiers(attributeModifier);
        }

        public void OnObserverOutsideOfRange(PillarObserver observer)
        {
            observer.RemoveModifiers(attributeModifier);
        }

        public void OnPillarActivate(PillarObserver observer)
        {
            observer.AddModifiers(attributeModifier);
        }

        public void SetPillarEmpowerState(PillarObserver observer, bool newState)
        {
            if (newState)
            {
                foreach (var item in empoweredAttributeModifier)
                {
                    observer.AddModifier(item);
                }
            }
        }
    }
}