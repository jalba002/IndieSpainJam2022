using UnityEngine;

namespace CosmosDefender
{
	public class PillarInteractionsController : MonoBehaviour
	{
        private PillarObserver pillarObserver;
        [SerializeField] private LayerMask pillarLayerMask;
        private PlayerInputs playerInputs;
        private PillarController selectedPillar;
        [SerializeField]
        private TargetButtonUI uiButton;
        private ResourceManager resourceManager;
        private void Awake()
        {
            pillarObserver = GetComponent<PillarObserver>();
            playerInputs = GetComponent<PlayerInputs>();
            resourceManager = GetComponent<ResourceManager>();
        }

        private void FixedUpdate()
        {
            if (pillarObserver.IsPillarInRange())
            {
                var ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
                if (Physics.Raycast(ray, out RaycastHit raycastHit, 15f, pillarLayerMask))
                {
                    var pillarController = raycastHit.collider.GetComponent<PillarController>();
                    selectedPillar = pillarController;
                    uiButton.SetState(true);
                    uiButton.SetButtonText(pillarController);
                }
                else
                {
                    selectedPillar = null;
                    uiButton.SetState(false);
                }
            }
            else
            {
                selectedPillar = null;
                uiButton.SetState(false);
            }
        }

        void OnInteractButton()
        {
            if (selectedPillar == null)
                return;

            selectedPillar.ActivatePillar();
            uiButton.SetButtonText(selectedPillar);
        }
    }
}