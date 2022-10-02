using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(PurchaseableSpellModifier),
        menuName = "CosmosDefender/" + nameof(PurchaseableSpellModifier))]
    public class PurchaseableSpellModifier : BasePurchaseableModifier<PurchaseableSpellModifierData, BaseSpellModifier,
        ISpellModifier, SpellData>
    {
#if UNITY_EDITOR

        private const string FolderName = "Assets/Game/Data/Shop/PurchaseableItems/Spells";
        [Button]
        private void CreateDefaultSetup()
        {
            // Create a folder with the name.
            // Create the three assets with the name associated.
            // Then just add them here, to the list.
            var a = Resources.FindObjectsOfTypeAll<PurchaseableSpellModifier>().Where(x => x == this).ToList();
            var splitAssetName = Utils.SplitByCamelCasing(a[0].name);
            var folderFinalName = splitAssetName[0];
            AssetDatabase.CreateFolder(FolderName, folderFinalName);
            
            PurchaseableSpellModifierData firstMod = new PurchaseableSpellModifierData()
            {
                Description = "This is the default description for a Tier 1 Mod.",
                Price = 75
            };
            
            PurchaseableSpellModifierData secondMod = new PurchaseableSpellModifierData()
            {
                Description = "This is the default description for a Tier 2 Mod.",
                Price = 100
            };
            
            PurchaseableSpellModifierData thirdMod = new PurchaseableSpellModifierData()
            {
                Description = "This is the default description for a Tier 3 Mod.",
                Price = 150
            };

            AssetDatabase.CreateAsset(firstMod, $"{FolderName}/{folderFinalName}/{splitAssetName[0]}{splitAssetName[1]}Upgrade_Tier1.asset");
            AssetDatabase.CreateAsset(secondMod, $"{FolderName}/{folderFinalName}/{splitAssetName[0]}{splitAssetName[1]}Upgrade_Tier2.asset");
            AssetDatabase.CreateAsset(thirdMod, $"{FolderName}/{folderFinalName}/{splitAssetName[0]}{splitAssetName[1]}Upgrade_Tier3.asset");

            modifers.Add(firstMod);
            modifers.Add(secondMod);
            modifers.Add(thirdMod);
        }
#endif
    }
}