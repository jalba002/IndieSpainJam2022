using TMPro;
using UnityEngine;

namespace CosmosDefender
{
	public class TargetButtonUI : MonoBehaviour
	{
        public TextMeshProUGUI ButtonText;
        public TextMeshProUGUI ButtonSubText;
        private bool isActive = false;

        public void SetState(bool newState)
        {
            isActive = newState;
            ButtonText.gameObject.SetActive(isActive);
        }

        public void SetButtonText(PillarController pillarController)
        {
            switch (pillarController.pillarCurrentState)
            {
                case PillarController.PillarStates.Inactive:
                    ButtonText.text = "(E) to activate\n" + pillarController.PillarName;
                    ButtonSubText.text = "Cost: " + pillarController.PillarConfig.ActivateCost;
                    break;
                case PillarController.PillarStates.Active:
                    ButtonText.text = "(E) to empower\n" + pillarController.PillarName;
                    ButtonSubText.text = "Cost: " + pillarController.PillarConfig.EmpowerCost;
                    break;
                case PillarController.PillarStates.Empowered:
                    ButtonText.text = "Empowered!";
                    ButtonSubText.text = "";
                    break;
            }
            
        }
    }
}