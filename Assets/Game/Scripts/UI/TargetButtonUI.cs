using System;
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
        [SerializeField] private LocalizeStringEvent textLocalized;

        public void SetState(bool newState)
        {
            textLocalized.gameObject.SetActive(newState);
        }

        public void SetButtonText(PillarController pillarController)
        {
            textLocalized.StringReference = new LocalizedString
            {
                {"tecla", new StringVariable() {Value = "E"}},
                {"value", new IntVariable() {Value = (int) pillarController.GetActivationCost()}},
                {"pillarName", new LocalizedString(pillarController.tableReference, pillarController.PillarName)}
            };
            
            textLocalized.SetTable("Gameplay");

            switch (pillarController.GetCurrentState())
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
        }
    }
}