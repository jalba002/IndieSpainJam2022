using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CosmosDefender
{
    public class TutorialPopUp : MonoBehaviour
    {
        [Header("Components")]

        [SerializeField] private LocalizeStringEvent title;
        [SerializeField] private LocalizeStringEvent description;
        [SerializeField] private LocalizeStringEvent dismiss;

        public void Configure(TutorialConfig settings)
        {
            //description.GetComponent<TMP_Text>().richText = true;
            
            title.SetTable(settings.tableCode);
            description.SetTable(settings.tableCode);
            dismiss.SetTable(settings.tableCode);
            
            title.SetEntry(settings.title);
            description.SetEntry(settings.description);
            dismiss.SetEntry(settings.dismiss);
        }
    }
}