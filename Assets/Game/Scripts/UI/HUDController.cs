using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace CosmosDefender
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private PlayerAttributes playerAtts;

        [Header("Prefabs")] [SerializeField] private HUDAbility abilityPrefab;

        [SerializeField] private HUDAbility goddessPrefab;

        [Header("Positions")] [SerializeField] private List<RectTransform> abilityPositions;

        [SerializeField] private RectTransform goddessPosition;

        private Dictionary<CosmosSpell, HUDAbility> instantiatedHudAbilities = new Dictionary<CosmosSpell, HUDAbility>();

        private List<RectTransform> availablePos;

        // TODO Initialize with other stuff.
        private void Start()
        {
            //InstantiateAllSpells();
            Initialize();
        }

        void Initialize()
        {
            playerAtts.OnSpellAdded += AddSpell;
            playerAtts.OnSpellUpdated += UpdateSpell;

            FindObjectOfType<SpellManager>().OnSpellCasted += ApplyCooldown; 
            
            availablePos = new List<RectTransform>();
            availablePos.AddRange(abilityPositions);
        }
        
        void AddSpell(CosmosSpell newSpell)
        {
            if (availablePos.Count <= 0)
            {
                Debug.LogWarning("Cannot add visual spell. No slot available.");
            }

            var hudInstance = Instantiate(abilityPrefab, availablePos[0].position, Quaternion.identity, this.transform);
            instantiatedHudAbilities.Add(newSpell, hudInstance);
            hudInstance.UpdateVisual(newSpell.GetSpell().spellData.AbilityIcon);

            availablePos.RemoveAt(0);
        }

        void UpdateSpell(CosmosSpell spell)
        {
            instantiatedHudAbilities.TryGetValue(spell, out HUDAbility hudReference);

            if (hudReference == null) return;
            
            hudReference.UpdateVisual(spell.GetSpell().spellData.AbilityIcon);
            hudReference.ApplyVisualCooldown(0f);
        }

        void ApplyCooldown(ISpell spell)
        {
            var a = instantiatedHudAbilities.Keys.ToList();
            var b = a.Find(x => x.GetSpell().spellData.GetHashCode() == spell.spellData.GetHashCode());
            instantiatedHudAbilities.TryGetValue(b, out HUDAbility hudReference);
            
            if (hudReference == null) return;

            hudReference.ApplyVisualCooldown(spell.spellData.Cooldown);
        }

        // void InstantiateAllSpells()
        // {
        //     var spells = playerAtts.GetAllSpells();
        //     for (int i = 0; i < spells.Count; i++)
        //     {
        //         var spell = spells[i].GetSpell();
        //         //spell.UpdateCurrentData();
        //
        //         var hudInstance = Instantiate(abilityPrefab, abilityPositions[i].position, Quaternion.identity,
        //             this.transform);
        //         instantiatedHudAbilities.Add(hudInstance);
        //         hudInstance.UpdateVisual(spell.spellData.AbilityIcon);
        //     }
        //
        //     var goddessInstance =
        //         Instantiate(abilityPrefab, goddessPosition.position, Quaternion.identity, this.transform);
        //     instantiatedHudAbilities.Add(goddessInstance);
        //     //goddessInstance.UpdateVisual(); // Goddess mode.
        // }

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