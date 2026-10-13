using UnityEngine;

public class MechMovement : MonoBehaviour
{
    [SerializeField] private float speed, acceleration;
    [SerializeField] private float dragFactor;

    private Rigidbody2D rb;
    private Vector2 wish;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ApplyDrag();
        ApplyControlAcceleration();
    }

    void ApplyDrag() {
        float magnitude = rb.linearVelocity.magnitude;
        if (magnitude < 0.3f) {
            rb.linearVelocity = Vector2.zero;
        } else {
            float drop = Mathf.Min(magnitude - dragFactor * Time.deltaTime) / magnitude;
            rb.linearVelocity *= drop;
        }
    }

    void ApplyControlAcceleration() {
        // for now just copy ship-like acceleration
         
        float curSpeed = Vector2.Dot(rb.linearVelocity, wish);
        float addSpeed = speed - curSpeed;

        if (addSpeed > 0) {
            float accelSpeed = Mathf.Min(acceleration * speed * Time.deltaTime, addSpeed);
            rb.linearVelocity += accelSpeed * wish;
        }
    }

    public void SetWishDirection(Vector2 _wish) { wish = _wish; }
}
