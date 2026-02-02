using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public UITest script;
    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public static bool optionsFromGame = false;
    void Awake()
    {
        Time.timeScale = 1f;
    }

    public void ReturnToMainMenu()
    {
        pauseMenu.SetActive(false);
        mainMenu.SetActive(true);
        UITest.mainMenuActive = true;
        optionsFromGame = false;
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        UITest.optionsMenuActive = true;
        optionsFromGame = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("Esc Pressed");
            pauseMenu.SetActive(false);
            Time.timeScale = 1f;
            UITest.gameActive = true;
            UITest.pauseMenuActive = false;
        }

        if (!UITest.gameActive)
        {
            Time.timeScale = 1f;
        }
    }
}
