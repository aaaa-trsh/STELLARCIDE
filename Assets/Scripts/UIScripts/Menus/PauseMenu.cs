using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    PlayerController playerController;

    private bool nested;
    void Awake()
    {
        playerController = GameManager.Instance.Player.GetComponent<PlayerController>();
        pauseMenu.SetActive(false);
        nested = false;
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        GameManager.Instance.GameActive = false;
        Time.timeScale = 0f;
        playerController.ToggleControls(false);
        EventBus.Instance.PauseGame(true);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.GameActive = true;
        playerController.ToggleControls(true);
        EventBus.Instance.PauseGame(false);
    }

    public void ReturnToMainMenu()
    {
        pauseMenu.SetActive(false);
        AudioManager.Instance.RestartBGM();
        playerController.ToggleControls(false);
        SceneManager.LoadScene("MainMenu");
    }

    public void Options()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        nested = true;
    }

    public void ExitOptions()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
        nested = false;
    }

    // TODO: move into InputActions Events
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.GameActive)
            {
                Pause();
            } 
            else if (!nested) 
            {
                Resume();
            }
        }
    }
}
