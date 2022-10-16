using UnityEngine;

namespace CosmosDefender
{
    public static class CanvasGroupExtensions
    {
        public static bool IsShown(this CanvasGroup canvasGroup)
        {
            return canvasGroup.alpha > 0;
        }
        
        public static void Show(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        public static void Hide(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}