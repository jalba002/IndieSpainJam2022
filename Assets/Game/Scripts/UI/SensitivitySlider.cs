using System;
using System.Collections;
using System.Collections.Generic;
using CosmosDefender;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private CosmosDefenderPlayerSettings _gameplaySettings;

    public TMP_Text sensitivityText;
    public Slider sensitivitySlider;

    private void Start()
    {
        float startingValue = _gameplaySettings.GetSensitivity();
        sensitivitySlider = GetComponent<Slider>();
        sensitivityText.text = startingValue.ToString("F2");
        sensitivitySlider.value = startingValue;
    }

    public void UpdateVisuals(float value)
    {
        sensitivityText.text = value.ToString("F2");
    }

    public void UpdateSensitivity(Single value)
    {
        _gameplaySettings.SetMouseSensitivity(value);
        
        UpdateVisuals(value);
    }
}
