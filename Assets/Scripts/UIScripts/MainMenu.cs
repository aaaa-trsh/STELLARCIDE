using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject optionsMenu;
    public GameObject mainMenu;
    public GameObject volumeMenu;
    public GameObject creditsMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void playGame()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void openOptionsMenu()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void returnToMainMenu()
    {         
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void openVolumeMenu()
    {
        optionsMenu.SetActive(false);
        volumeMenu.SetActive(true);
    }

    public void returnToOptionsMenu()
    {
        volumeMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void quitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
