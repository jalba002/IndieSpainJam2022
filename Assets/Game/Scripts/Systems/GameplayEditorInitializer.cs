#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
#endif
using UnityEngine;

namespace CosmosDefender
{
    public class GameplayEditorInitializer : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField]
        private List<CosmosSpell> allSpells;
        [SerializeField]
        private PlayerAttributes playerAttributes;
        [SerializeField]
        private CosmosSpell basicAttack;
        [SerializeField]
        private bool startWithoutModifiers;
        [SerializeField]
        private bool forceInitialize = true;

        private void Awake()
        {
            foreach (var item in allSpells)
            {
                item.isSpellEmpowered = false;
            }

            playerAttributes.Initialize(forceInitialize);
            playerAttributes.RemoveAllSpells();
            playerAttributes.AddSpell(basicAttack);
            allSpells.ForEach(playerAttributes.AddSpell);

            if (startWithoutModifiers)
            {
                playerAttributes.RemoveAllAttributeModifiers();
                playerAttributes.RemoveAllSpellModifiers();
            }
        }

        private void Reset()
        {
            var GUIDS = AssetDatabase.FindAssets("t: " + nameof(CosmosSpell));
            var paths = GUIDS.Select(AssetDatabase.GUIDToAssetPath);
            allSpells = paths.Select(AssetDatabase.LoadAssetAtPath<CosmosSpell>).ToList();

            playerAttributes = AssetDatabase.LoadAssetAtPath<PlayerAttributes>(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("t: " + nameof(PlayerAttributes))[0]));
        }
#endif
    }
}