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

        [SerializeField]
        private List<BaseTemporalAttributeModifier> goddessAttributeModifier = new List<BaseTemporalAttributeModifier>();

        public void OnGoddessActive(PillarObserver observer)
        {
            foreach (var item in goddessAttributeModifier)
            {
                observer.AddModifier(item);
            }
        }

        public void OnGoddessUnactive(PillarObserver observer)
        {
            //Modifier is temporal, so no need to do anything on goddess mode becoming unactive
        }

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
            if (!GameManager.Instance.hasActivatedFirstPasivePillar)
            {
                TutorialPopUpManager.Instance.ActivateTutorial(1, 1f);
                GameManager.Instance.hasActivatedFirstPasivePillar = true;
            }
        }

        public void SetPillarEmpowerState(PillarObserver observer, bool newState)
        {
            if (newState)
            {
                foreach (var item in empoweredAttributeModifier)
                {
                    observer.AddModifier(item);
                }

                if (!GameManager.Instance.hasEmpoweredFirstPillar)
                {
                    TutorialPopUpManager.Instance.ActivateTutorial(3, 1f);
                    GameManager.Instance.hasEmpoweredFirstPillar = true;
                }
            }
        }
    }
}