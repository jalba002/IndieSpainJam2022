using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CosmosDefender
{
    [CreateAssetMenu(fileName = nameof(EconomyConfig), menuName = "CosmosDefender/" + nameof(EconomyConfig))]
    public class EconomyConfig : ScriptableObject
    {
        private const string EconomyKey = nameof(EconomyKey);

        [SerializeField]
        private EconomyData economy;

        public void Initialize()
        {
            Load();
        }

        [Button]
        public void AddMoney(int amount)
        {
            economy.money += amount;
            Save();
        }

        [Button]
        public void SubstractMoney(int amount)
        {
            economy.money -= amount;
            Save();
        }

        public float GetMoney() => economy.money;

        private void Load()
        {
            economy = JsonUtility.FromJson<EconomyData>(PlayerPrefs.GetString(EconomyKey, ""));
        }

        private void Save()
        {
            PlayerPrefs.SetString(EconomyKey, JsonUtility.ToJson(economy));
        }
    }

    [Serializable]
    public struct EconomyData
    {
        public int money;
    }
}