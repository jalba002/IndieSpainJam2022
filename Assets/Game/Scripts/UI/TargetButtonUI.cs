using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Localization.Tables;

namespace CosmosDefender
{
    public class TargetButtonUI : MonoBehaviour
    {
        private bool isActive = false;
        [SerializeField] LocalizeStringEvent textLocalized;
        [SerializeField] private TableReference localizationTable;

        public void SetState(bool newState)
        {
            isActive = newState;
            textLocalized.gameObject.SetActive(isActive);
        }

        public void SetButtonText(PillarController pillarController)
        {
            switch (pillarController.pillarCurrentState)
            {
                case PillarController.PillarStates.Inactive:
                    textLocalized.SetEntry("PilarInactivo");
                    break;
                case PillarController.PillarStates.Active:
                    textLocalized.SetEntry("PilarActivo");
                    break;
                case PillarController.PillarStates.Empowered:
                    textLocalized.SetEntry("PilarEmpoderado");
                    break;
            }
            
            textLocalized.StringReference.Add("key", new StringVariable() {Value = "E"});
            textLocalized.StringReference.Add("value", new IntVariable() {Value = (int) pillarController.PillarConfig.ActivateCost});
            textLocalized.StringReference.Add("pillarName", new LocalizedString(localizationTable, pillarController.PillarName));
        }
    }
}