using Unity.VisualScripting;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public PauseMenu script;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject volumeMenu;
    public GameObject pauseMenu;
    public GameObject returnToMenu;
    public GameObject returnToGame;
    public static bool mainMenuActive = true;
    public static bool optionsMenuActive = false;
    public static bool gameActive = false;
    public static bool pauseMenuActive = false;
    public AudioSource click;
    void Awake()
    {
        Time.timeScale = 0f;
    }
    public void Play()
    {
        //Debug.Log("This is a test script for UI.");
        Time.timeScale = 1f;
        gameActive = true;
        mainMenu.SetActive(false);
    }

    public void Click()
    {
        click.Play();
    }
    public void Options()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        optionsMenuActive = true;
        mainMenuActive = false;
    }

    public void VolumeMenu()
    {
        optionsMenu.SetActive(false);
        volumeMenu.SetActive(true);
        optionsMenuActive = false;
    }

    public void Return()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        optionsMenuActive = false;
        mainMenuActive = true;
    }

    public void ReturnFromVolume()
    {
        optionsMenu.SetActive(true);
        volumeMenu.SetActive(false);
        mainMenuActive = false;
        optionsMenuActive = true;
    }

    public void ReturnToGame()
    {
        optionsMenu.SetActive(false);
        optionsMenuActive = false;
        mainMenuActive = false;
        pauseMenuActive = false;
        gameActive = true;
        Time.timeScale = 1f;
        //Debug.Log("Retured");
    }

    private void Update()
    {
        if (mainMenuActive || optionsMenuActive || pauseMenuActive)
        {
            //Debug.Log("Something is Active");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !mainMenuActive && !optionsMenuActive)
        {
            //Debug.Log("Esc Pressed");
            pauseMenu.SetActive(true);
            pauseMenuActive = true;
            gameActive = false;
            //Debug.Log("Game is not active");
        }
        if (PauseMenu.optionsFromGame)
        {
            returnToMenu.SetActive(false);
            returnToGame.SetActive(true);
        }
        else
        {
            returnToMenu.SetActive(true);
            returnToGame.SetActive(false);
        }
        if (gameActive)
        {
            //Debug.Log("GameActive");
            Time.timeScale = 1f;
            Cursor.visible = false;
            mainMenuActive = false;
            optionsMenuActive = false;
            pauseMenuActive = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
