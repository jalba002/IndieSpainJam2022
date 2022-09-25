using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(TutorialConfig), menuName = "CosmosDefender/UI/" + nameof(TutorialConfig))]
    public class TutorialConfig : ScriptableObject
    {
        public string title = "Default";

        [TextArea(20, 60)] public string description = "";

        public string dismiss =
            "<size=30><color=green>ESC</color></size> o <size=30 ><color=green>START</color></size> para continuar.";
    }
}