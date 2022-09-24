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
            group.Show();
            onShow.Invoke();
        }

        public void Hide()
        {
            group.Hide();
            onHide.Invoke();
        }
    }
}