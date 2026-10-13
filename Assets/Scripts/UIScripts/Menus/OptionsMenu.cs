using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    public PauseMenu pauseMenu;
    public GameObject optionsMenu;
    public GameObject volumeMenu;
    public GameObject graphicsMenu;
    [SerializeField] bool inMainMenu = false;
    private bool nested;

    private void Awake()
    {
        nested = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Return()
    {
        if (!inMainMenu)
            pauseMenu.ExitOptions();
        else SceneManager.LoadScene("MainMenu");
    }

    public void EnterVolume()
    {
        optionsMenu.SetActive(false);
        volumeMenu.SetActive(true);
        nested = true;
    }

    public void ExitVolume()
    {
        optionsMenu.SetActive(true);
        volumeMenu.SetActive(false);
        nested = false;
    }
    
    public void EnterGraphics()
    {
        optionsMenu.SetActive(false);
        graphicsMenu.SetActive(true);
        nested = true;
    }

    public void ExitGraphics()
    {
        optionsMenu.SetActive(true);
        graphicsMenu.SetActive(false);
        nested = false;
    }

    public void ResetTutorial()
    {
        PlayerPrefs.SetString("TutorialFinished", "no");
    }
}
