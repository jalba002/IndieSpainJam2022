using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
	public class PillarSkillModifierBehavior : MonoBehaviour, IPillarObserverModifier
	{
        [SerializeField]
        private CosmosSpell pillarSpell;

        private SpellManager spellController;
        
        [Header("Tutorial")]
        [SerializeField] private TutorialConfig firstPillar;
        [SerializeField] private TutorialConfig empowerPillar;

        private void Awake()
        {
            spellController = FindObjectOfType<SpellManager>();
        }

        public void OnObserverInRange(PillarObserver observer)
        {
            //observer.SetSpellEmpowerState(pillarSpell, true);
        }

        public void OnObserverOutsideOfRange(PillarObserver observer)
        {
            //observer.SetSpellEmpowerState(pillarSpell, false);
        }

        public void OnPillarActivate(PillarObserver observer)
        {
            observer.AddSpell(pillarSpell);
            if (!GameManager.Instance.hasActivatedFirstSkillPillar)
            {
                TutorialPopUpManager.Instance.ActivateTutorial(firstPillar, 1f);
                //GameManager.Instance.hasActivatedFirstSkillPillar = true;
                GameManager.Instance.Save(nameof(GameManager.Instance.hasActivatedFirstSkillPillar), GameManager.Instance.hasActivatedFirstSkillPillar = true);
            }
        }

        public void SetPillarEmpowerState(PillarObserver observer, bool newState)
        {
            if (newState)
            {
                if (!GameManager.Instance.hasEmpoweredFirstPillar)
                {
                    TutorialPopUpManager.Instance.ActivateTutorial(empowerPillar, 1f);
                    GameManager.Instance.hasEmpoweredFirstPillar = true;
                }
            }
            observer.SetSpellEmpowerState(pillarSpell, newState);
        }

        public void OnGoddessActive(PillarObserver observer)
        {
            observer.SetSpellEmpowerState(pillarSpell, true);
        }

        public void OnGoddessUnactive(PillarObserver observer)
        {
            observer.SetSpellEmpowerState(pillarSpell, false);
        }
    }
}