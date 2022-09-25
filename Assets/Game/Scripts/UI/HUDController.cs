using System;
using System.Collections.Generic;
using UnityEngine;

namespace CosmosDefender
{
	public class HUDController : MonoBehaviour
	{
		[SerializeField]
		private PlayerAttributes playerAtts;

		[Header("Prefabs")]
		[SerializeField] private HUDAbility abilityPrefab;
		
		[SerializeField] private HUDAbility goddessPrefab;
		
		[Header("Positions")]
		[SerializeField] private List<RectTransform> abilityPositions;

		[SerializeField] private RectTransform goddessPosition;

		private List<HUDAbility> instantiatedHudAbilities = new List<HUDAbility>();

		private void Start()
		{
			InstantiateAllSpells();
		}

		void InstantiateAllSpells()
		{
			var spells = playerAtts.GetAllSpells();
			for (int i = 0; i < spells.Count; i++)
			{
				var spell = spells[i].GetSpell();
				//spell.UpdateCurrentData();
				
				var hudInstance = Instantiate(abilityPrefab, abilityPositions[i].position, Quaternion.identity, this.transform);
				instantiatedHudAbilities.Add(hudInstance);
				hudInstance.UpdateVisual(spell.spellData.AbilityIcon);
			}
			
			var goddessInstance = Instantiate(abilityPrefab, goddessPosition.position, Quaternion.identity, this.transform);
			instantiatedHudAbilities.Add(goddessInstance);
			//goddessInstance.UpdateVisual(); // Goddess mode.
		}

		public void PlaceOnCooldown(SpellKeyType spellKey)
		{
			if (!playerAtts.HasSpellKey(spellKey))
				return;
			
			var selectedSpell = playerAtts.GetSpell(spellKey);

			// When having the selected spell, cycle through all HUDAbilty
			// Then call .applyCooldown on it.
			// The rest will do itself.
			
			// When an update comes, update with time so.
		}
	}
}