using UnityEngine;
using FMOD.Studio;
using UnityEngine.UI;

public class VcaController : MonoBehaviour
{
    private VCA Controller;
    public string VcaName;

    private Slider slider;

    void Start()
    {
        float startingVolume = PlayerPrefs.GetFloat(VcaName + "Volume", 0.5f);
        Controller = FMODUnity.RuntimeManager.GetVCA("vca:/" + VcaName);
        slider = GetComponent<Slider>();
        Controller.setVolume(startingVolume);
        slider.value = startingVolume;
    }

    public void SetVolume(float newVolume)
    {
        Controller.setVolume(newVolume);
        PlayerPrefs.SetFloat(VcaName + "Volume", newVolume);
    }
}