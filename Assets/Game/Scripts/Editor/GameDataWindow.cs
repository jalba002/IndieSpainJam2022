using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace CosmosDefender
{
    public class GameDataWindow : OdinMenuEditorWindow
    {
        [MenuItem("CosmosDefender/GameData")]
        private static void Open()
        {
            GetWindow<GameDataWindow>();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.AddAllAssetsAtPath("Data", "Game/Data", true, false);
            return tree;
        }
    }
}