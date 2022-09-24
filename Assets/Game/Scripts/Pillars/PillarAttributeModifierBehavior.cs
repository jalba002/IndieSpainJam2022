using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
    public class PillarAttributeModifierBehavior : MonoBehaviour, IPillarObserverModifier
    {
        [SerializeField]
        private List<BaseAttributeModifier> attributeModifier = new List<BaseAttributeModifier>();

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
            
        }

        public void SetPillarEmpowerState(PillarObserver observer, bool newState)
        {
            Debug.Log("Empower State Not Implemented Yet!");
        }
    }
}