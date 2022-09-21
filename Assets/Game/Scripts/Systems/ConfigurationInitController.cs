using UnityEngine;
using UnityEngine.SceneManagement;

namespace CosmosDefender
{
    public class ConfigurationInitController : MonoBehaviour
    {
        [SerializeField]
        private PlayerAttributes playerAttributes;
        [SerializeField]
        private string menuController;

        private void Awake()
        {
            playerAttributes.Initialize();
        }

        private void Start()
        {
            SceneManager.LoadScene(menuController);
        }
    }
}