using UnityEngine;

namespace CosmosDefender
{
    public class LanguageSwitcher : MonoBehaviour
    {
        [SerializeField] private CosmosDefenderPlayerSettings cosmosDefenderUserSettings;
  
        public void LocaleSelected(string localeName)
        {
            cosmosDefenderUserSettings.SetLanguage(localeName);
        }
    }
}