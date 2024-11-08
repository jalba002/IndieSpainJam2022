using UnityEngine;
using UnityEngine.SceneManagement;

namespace CosmosDefender
{
    public class ConfigurationInitController : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttributes playerAttributes;
        [SerializeField]
        private ShopModifiers shopAttributes;
        [SerializeField]
        private EconomyConfig economyConfig;
        [SerializeField]
        private string menuController;

        private void Awake()
        {
            playerAttributes.Initialize(true);
            shopAttributes.Initialize();
            economyConfig.Initialize();
        }

        private void Start()
        {
            SceneManager.LoadScene(menuController);
        }
    }
}