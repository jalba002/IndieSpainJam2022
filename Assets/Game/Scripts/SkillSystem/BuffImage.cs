using UnityEngine;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class BuffImage : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        public void Show(Sprite sprite)
        {
            image.sprite = sprite;
        }
    }
}