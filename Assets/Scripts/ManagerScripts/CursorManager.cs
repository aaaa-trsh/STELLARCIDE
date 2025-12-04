using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private bool gameScene = true;

    private void Start()
    {
        if (gameScene)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else 
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
