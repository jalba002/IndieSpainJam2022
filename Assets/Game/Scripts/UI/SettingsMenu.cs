using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public TMPro.TMP_Dropdown resolutionDropdown;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        int iter = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (options.Contains(option.ToLower()))
            {
                continue;
            }

            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = iter;
            }
            iter++;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        var width = int.Parse(resolutionDropdown.options[resolutionIndex].text.Split('x')[0]);
        var height = int.Parse(resolutionDropdown.options[resolutionIndex].text.Split('x')[1]);

        Screen.SetResolution(width, height, Screen.fullScreen);
    }
}
