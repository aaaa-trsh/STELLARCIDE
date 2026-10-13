using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsMenu : MonoBehaviour
{
    // reference to options menu parent
    public OptionsMenu optionsMenu;
    // TODO add references to panel
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle VsyncToggle;
    // List of graphics options
    private Resolution[] _resolutions;
    int selectedResolution;
    private List<Resolution> selectedResolutionList = new List<Resolution>();
    
    private void Awake()
    {
        _resolutions = Screen.resolutions;
    }

    private void Start()
    {
        VsyncToggle.isOn = PlayerPrefs.GetInt("Vsync") == 1;
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
        
        List<string> resolutionStringList = new List<string>();
        string newRes;
        foreach (Resolution resolution in _resolutions)
        {
            newRes = resolution.width + " x " + resolution.height;
            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                selectedResolutionList.Add(resolution);
            }
        }
        
        resolutionDropdown.AddOptions(resolutionStringList);
        
        // set displayed value
        int count = 0;
        foreach (Resolution resolution in Screen.resolutions)
        {
            
            if (resolution.width == PlayerPrefs.GetInt("ResolutionWidth") && resolution.height == PlayerPrefs.GetInt("ResolutionHeight"))
            {
                selectedResolution = count;
            }
            count++;
        }
        
        resolutionDropdown.value = selectedResolution;
    }

    // return to options menu
    public void Return()
    {
        optionsMenu.ExitGraphics();
    }
    
    // TODO grab the graphics and change them!
    public void GraphicsSelect()
    {
        selectedResolution = resolutionDropdown.value;
        Screen.SetResolution(selectedResolutionList[selectedResolution].width, selectedResolutionList[selectedResolution].height, fullscreenToggle.isOn);
        PlayerPrefs.SetInt("ResolutionWidth", selectedResolutionList[selectedResolution].width);
        PlayerPrefs.SetInt("ResolutionHeight", selectedResolutionList[selectedResolution].height);
    }

    public void FullscreenSelect()
    {
        int temp = (PlayerPrefs.GetInt("Fullscreen") + 1) % 2;
        PlayerPrefs.SetInt("Fullscreen", temp);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen") == 1;
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        PlayerPrefs.Save();
    }

    public void VsyncSelect()
    {
        int temp = (PlayerPrefs.GetInt("Vsync") + 1) % 2;
        PlayerPrefs.SetInt("Vsync", temp);
        VsyncToggle.isOn = PlayerPrefs.GetInt("Vsync") == 1;
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("VSync");
        PlayerPrefs.Save();
    }
    
}
