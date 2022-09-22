using UnityEngine;

namespace CosmosDefender
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasMenu : MonoBehaviour
    {
        private CanvasGroup group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        public void Show()
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }

        public void Hide()
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }
}