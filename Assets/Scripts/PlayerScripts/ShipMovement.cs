using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float speed, acceleration;
    [SerializeField] private float swingReduceFactor;
    [SerializeField] private float driftReduceFactor;
    // [SerializeField] private float boost = 5;

    private Rigidbody2D rb;
    private Vector2 wish;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        ApplySwingReduction();
        ApplyDriftReduction();
        ApplyControlAcceleration();
    }

    void ApplySwingReduction() {       
        // add drag to the component of velocity that is not along the ships forward axis
        Vector2 swingReduction = -swingReduceFactor * Time.deltaTime * (rb.linearVelocity - (Vector2)transform.right * Vector2.Dot(transform.right, rb.linearVelocity));
        if (wish.magnitude != 0 && rb.linearVelocity.magnitude > 0.3) {
            rb.linearVelocity += swingReduction;
        }
    }

    void ApplyDriftReduction() {
        //if the ship is hurling away from the cursor, boost it a bit
        float dot = Vector2.Dot(rb.linearVelocity.normalized, wish);
        if (wish.magnitude != 0 && dot < 0) {
            rb.linearVelocity += wish * Mathf.Abs(dot) * driftReduceFactor;
        }
    }

    void ApplyControlAcceleration() {
        // valve style acceleration: add limit the projection of wish onto velocity 
        float curSpeed = Vector2.Dot(rb.linearVelocity, wish);
        float addSpeed = speed - curSpeed;

        if (addSpeed > 0) {
            float accelSpeed = Mathf.Min(acceleration * speed * Time.deltaTime, addSpeed);
            rb.linearVelocity += accelSpeed * wish;
        }
    }

    public void SetWishDirection(Vector2 _wish) { wish = _wish; }
}
