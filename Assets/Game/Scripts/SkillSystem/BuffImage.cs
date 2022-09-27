using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CosmosDefender
{
    public class BuffImage : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField] private List<Image> tierElements;

        public void Show(Sprite sprite, int tierLevel = 0)
        {
            image.sprite = sprite;
            for (int i = 0; i < tierLevel; i++)
            {
                tierElements[i].enabled = true;
            }
        }

        private void OnDisable()
        {
            foreach (var plus in tierElements)
            {
                plus.enabled = false;
            }
        }
    }
}