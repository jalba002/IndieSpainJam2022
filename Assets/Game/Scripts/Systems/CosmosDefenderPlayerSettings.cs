using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(CosmosDefenderPlayerSettings),
        menuName = "CosmosDefender/" + nameof(CosmosDefenderPlayerSettings))]
    public class CosmosDefenderPlayerSettings : ScriptableObject
    {
        private const string CosmosSettingsKey = nameof(CosmosSettingsKey);

        [SerializeField] private CosmosDefenderPlayerSettingsData data;

        public Action OnSettingsUpdated;

        public void Initialize()
        {
            data = new CosmosDefenderPlayerSettingsData()
            {
                MouseSensitivity = 10f,
                LanguagePreference = "English"
            };
            Load(data);
        }

        public void SetLanguage(string language)
        {
            var languagePack = LocalizationSettings.AvailableLocales.Locales.Find(x => x.LocaleName.Contains(language));

            if (languagePack == null) return;

            LocalizationSettings.SelectedLocale = languagePack;
            data.LanguagePreference = language;
            Save();
        }

        public void SetMouseSensitivity(float value)
        {
            data.MouseSensitivity = value;
            Save();
        }

        public string GetLanguage() => LocalizationSettings.SelectedLocale.LocaleName;
        public float GetSensitivity() => data.MouseSensitivity;

        private void Load(CosmosDefenderPlayerSettingsData settings)
        {
            try
            {
                var loadedData = JsonUtility.FromJson<CosmosDefenderPlayerSettingsData>(PlayerPrefs.GetString(CosmosSettingsKey));

                var languagePack = LocalizationSettings.AvailableLocales.Locales.Find(x => x.LocaleName.Contains(data.LanguagePreference));

                Debug.Log(loadedData.MouseSensitivity);
                data = loadedData;
                
                if (languagePack == null) return;
                LocalizationSettings.SelectedLocale = languagePack;
            }
            catch (NullReferenceException)
            {
                // Generate without ref in regedit.
                var languagePack =
                    LocalizationSettings.AvailableLocales.Locales.Find(x =>
                        x.LocaleName.Contains(settings.LanguagePreference));

                if (languagePack == null) return;
                LocalizationSettings.SelectedLocale = languagePack;
            }
        }

        [Button]
        private void Save()
        {
            PlayerPrefs.SetString(CosmosSettingsKey, JsonUtility.ToJson(data));
            OnSettingsUpdated?.Invoke();
        }
    }

    [Serializable]
    public struct CosmosDefenderPlayerSettingsData
    {
        public string LanguagePreference;
        public float MouseSensitivity;
    }
}