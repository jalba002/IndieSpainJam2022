using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class TutorialPopUp : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;
        [SerializeField] private TMP_Text dismiss;

        public void Configure(TutorialConfig settings)
        {
            title.text = settings.title;
            description.richText = true;
            description.text = settings.description;
            dismiss.text = settings.dismiss;
        }
    }
}