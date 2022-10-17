using System.Linq;
using CosmosDefender.Shop;
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

            for (int i = 0; i < 3; i++)
            {
                PurchaseableSpellModifierData mod = ScriptableObject.CreateInstance<PurchaseableSpellModifierData>();
                mod.Price = (75 + (25 * i));
                AssetDatabase.CreateAsset(mod, $"{FolderName}/{folderFinalName}/{splitAssetName[0]}{splitAssetName[1]}Upgrade_Tier{i + 1}.asset");
                modifers.Add(mod);
            }
        }
#endif
    }
}