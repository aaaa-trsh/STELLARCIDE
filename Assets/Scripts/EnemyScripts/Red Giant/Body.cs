using UnityEngine;

public class Body : MonoBehaviour
{
    public Transform target;       // The segment in front
    public float followDistance = 0.5f;
    public float followSpeed = 5f; // How fast the segment catches up

    private Vector2 lastPos;
    public bool freeze = false;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        if (!target || freeze) return;

        // Compute the desired position at the correct follow distance
        Vector2 dir = (Vector2)(target.position - transform.position);
        Vector2 desiredPos = (Vector2)target.position - dir.normalized * followDistance;

        // Only move forward (prevents tail from going backward)
        Vector2 delta = desiredPos - lastPos;
        Vector2 forwardDir = dir.normalized;
        float forwardMovement = Vector2.Dot(delta, forwardDir);

        if (forwardMovement > 0f)
        {
            Vector2 move = forwardDir * forwardMovement;

            // Smoothly interpolate instead of snapping
            transform.position = Vector2.MoveTowards(transform.position, lastPos + move, followSpeed * Time.deltaTime);
        }

        // Rotate toward the segment in front
        transform.up = dir.normalized;

        // Update last position
        lastPos = transform.position;
    }
}
