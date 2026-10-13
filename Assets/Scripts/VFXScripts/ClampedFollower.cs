using UnityEngine;

public class ClampedFollower : MonoBehaviour
{
    public Transform target;
    public float maxDistance = 10f;

    void Update() {
        if (maxDistance > 0.1f) {
            transform.position = transform.parent.position + Vector3.ClampMagnitude(target.position - transform.parent.position, maxDistance);
        } else {
            transform.position = transform.parent.position;
        }
    }
}