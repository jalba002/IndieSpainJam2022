using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
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
            Load();
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

        private void Load()
        {
            try
            {
                data = JsonUtility.FromJson<CosmosDefenderPlayerSettingsData>(
                    PlayerPrefs.GetString(CosmosSettingsKey, "{}"));

                var languagePack =
                    LocalizationSettings.AvailableLocales.Locales.Find(x =>
                        x.LocaleName.Contains(data.LanguagePreference));

                if (languagePack == null) return;
                LocalizationSettings.SelectedLocale = languagePack;
            }
            catch (ArgumentNullException)
            {
                // Generate without ref in regedit.
                Save();
            }
        }

        [Button]
        private void Save()
        {
            OnSettingsUpdated?.Invoke();
            PlayerPrefs.SetString(CosmosSettingsKey, JsonUtility.ToJson(data));
        }
    }

    [Serializable]
    public struct CosmosDefenderPlayerSettingsData
    {
        public string LanguagePreference;
        public float MouseSensitivity;

        public CosmosDefenderPlayerSettingsData()
        {
            LanguagePreference = "English";
            MouseSensitivity = 1f;
        }
    }
}