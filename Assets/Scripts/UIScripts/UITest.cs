using Unity.VisualScripting;
using UnityEngine;

public class UITest : MonoBehaviour
{
    public GameObject mainMenu;
    public bool mainMenuActive = true;
    public AudioSource click;
    void Awake()
    {
        Time.timeScale = 0f;
    }
    public void Play()
    {
        click.Play();
        Debug.Log("This is a test script for UI.");
        Time.timeScale = 1f;
        mainMenu.SetActive(false);
        mainMenuActive = false;
    }

    private void Update()
    {
        if (mainMenuActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
