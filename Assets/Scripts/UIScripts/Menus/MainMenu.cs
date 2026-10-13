using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    int resolutionWidth;
    int resolutionHeight;
    bool fullscreenToggled;
    private void Start()
    {
        // set tutorial preferences
        if (!PlayerPrefs.HasKey("TutorialFinished"))
        {
            PlayerPrefs.SetString("TutorialFinished", "no");
        }

        // set option preferences for first time
        if (!PlayerPrefs.HasKey("ResolutionWidth"))
        {
            PlayerPrefs.SetInt("ResolutionWidth", Screen.width);
            resolutionWidth = PlayerPrefs.GetInt("ResolutionWidth");
        }
        resolutionWidth = PlayerPrefs.GetInt("ResolutionWidth");
        if (!PlayerPrefs.HasKey("ResolutionHeight"))
        {
            PlayerPrefs.SetInt("ResolutionHeight", Screen.height);
            
        }
        resolutionHeight = PlayerPrefs.GetInt("ResolutionHeight");
        
        if (!PlayerPrefs.HasKey("Fullscreen"))
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        fullscreenToggled = PlayerPrefs.GetInt("Fullscreen") == 1;
        
        Screen.SetResolution(resolutionWidth, resolutionHeight, fullscreenToggled);

        if (!PlayerPrefs.HasKey("Vsync"))
        {
            PlayerPrefs.SetInt("Vsync", 1);
        }

        if (!PlayerPrefs.HasKey("NewGame"))
        {
            PlayerPrefs.SetInt("NewGame", 0);
        }
        // New game denotes clearing the map json (for the default demo we will not have saved games across loads)

        PlayerPrefs.SetString("PlayerPosition", "0");

        PlayerPrefs.Save();
    }

    public void Play()
    {
        PlayerPrefs.SetInt("NewGame", 1);
        // check for tutorial flag
        if (PlayerPrefs.GetString("TutorialFinished") == "no") SceneManager.LoadScene("Tutorial");
        // otherwise play game again
        else SceneManager.LoadScene("AreaSelectionMap");
        Time.timeScale = 1;
    }

    public void Options()
    {
        SceneManager.LoadScene("Options");
    }
    
    public void Leaderboard() 
    {
        SceneManager.LoadScene("LeaderboardMenu");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Quit()
    {
        Application.Quit();
    }

    
}
