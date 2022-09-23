using UnityEngine;
using UnityEngine.Events;

namespace CosmosDefender
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CanvasMenu : MonoBehaviour
    {
        public UnityEvent onShow;
        public UnityEvent onHide;
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
            onShow.Invoke();
        }

        public void Hide()
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            onHide.Invoke();
        }
    }
}