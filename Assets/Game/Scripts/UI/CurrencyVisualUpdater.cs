using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CosmosDefender
{
    public class CurrencyVisualUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Text moneymoneymoney;

        [SerializeField] private EconomyConfig elbanquero;

        [Range(0, 10)] [SerializeField]
        private int amountOfZeroes = 6;

        public void Start()
        {
            elbanquero.OnMoneyUpdated += UpdateMoney;
        }

        void UpdateMoney(int dineros)
        {
            var count = dineros.ToString().ToCharArray().Length;
            var zeroes = amountOfZeroes - count;

            string finalVerdeEnLosPantalones = "";
            for(int i = 0; i<zeroes; i++)
            {
                finalVerdeEnLosPantalones += "0";
            }

            finalVerdeEnLosPantalones += dineros.ToString();
            moneymoneymoney.text = finalVerdeEnLosPantalones;
        }

        private void OnEnable()
        {
            UpdateMoney(elbanquero.GetMoney());
        }

        private void OnValidate()
        {
            UpdateMoney(elbanquero.GetMoney());
        }
    }
}