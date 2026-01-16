using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject optionsMenu;
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
