using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject mainmenu;
    public GameObject optionsmenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Time.timeScale = 0f;
    }
    
    public void startGame()
    {
        mainmenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void openOptionsMenu()
    {
       optionsmenu.SetActive(true);
       mainmenu.SetActive(false);
    }

}