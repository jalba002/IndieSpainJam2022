using TMPro;
using UnityEngine;

namespace CosmosDefender
{
	public class TargetButtonUI : MonoBehaviour
	{
        public TextMeshProUGUI ButtonText;
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
                    ButtonText.text = 
                        $"<color=green>(E)</color> para activar un\n{pillarController.PillarName}\nCoste: <color=orange>{pillarController.PillarConfig.ActivateCost}</color> estrellas.";
                    break;
                case PillarController.PillarStates.Active:
                    ButtonText.text =
                        $"<color=green>(E)</color> para empoderar un\n{pillarController.PillarName}\nCoste: <color=orange>{pillarController.PillarConfig.EmpowerCost}</color> estrellas."; 
                    break;
                case PillarController.PillarStates.Empowered:
                    ButtonText.text = "<size=52>PILAR EMPODERADO!</size>";
                    break;
            }
            
        }
    }
}