using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
	public class PillarSkillModifierBehavior : MonoBehaviour, IPillarObserverModifier
	{
        [SerializeField]
        private CosmosSpell pillarSpell;

        private SpellManager spellController;

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
        }

        public void SetPillarEmpowerState(PillarObserver observer, bool newState)
        {
            observer.SetSpellEmpowerState(pillarSpell, newState);
        }
    }
}