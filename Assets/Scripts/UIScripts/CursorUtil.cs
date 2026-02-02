using UnityEngine;

public class CursorUtil : MonoBehaviour
{
    public static Vector2 GetCursorPosition() {
        return Camera.main.ScreenToWorldPoint(
            new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                0
            )
        );
    }

    void FixedUpdate() {
        Cursor.visible = false;
        transform.position = GetCursorPosition();
    }
}