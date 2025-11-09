using System;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float speed, acceleration;
    [SerializeField] private float swingReduceFactor;
    [SerializeField] private float driftReduceFactor;
    [SerializeField] private float thrustCamSpeed = 5;

    private Rigidbody2D rb;
    
    private float thrustInput;
    private Vector2 wish;

    private ClampedFollower camTarget;
    private float initialCamTargetDist = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        camTarget = GetComponentInChildren<ClampedFollower>();
        initialCamTargetDist = camTarget.maxDistance;
    }

    void Update() {
        thrustInput = Input.GetKey(KeyCode.Space) ? 1 : 0;
        wish = transform.right * thrustInput;

        if (Input.GetKeyDown(KeyCode.R)) {
            transform.position = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
        }

        // turn towards cursor
        Vector3 cursor = CursorUtil.GetCursorPosition();
        Vector3 direction = cursor - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);        
        
        if (thrustInput > 0) {   
            camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, initialCamTargetDist, Time.deltaTime * thrustCamSpeed);
        } else {
            camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, 0, Time.deltaTime * thrustCamSpeed);;
        }

        ApplySwingReduction();
        ApplyDriftReduction();
        ApplyControlAcceleration();
    }

    void ApplySwingReduction() {       
        // add drag to the component of velocity that is not along the ships forward axis
        Vector2 swingReduction = -swingReduceFactor * Time.deltaTime * (rb.linearVelocity - (Vector2)transform.right * Vector2.Dot(transform.right, rb.linearVelocity));
        if (thrustInput != 0 && rb.linearVelocity.magnitude > 0.3) {
            rb.linearVelocity += swingReduction;
        }
    }

    void ApplyDriftReduction() {
        //if the ship is hurling away from the cursor, boost it a bit
        float dot = Vector2.Dot(rb.linearVelocity.normalized, wish);
        if (thrustInput != 0 && dot < 0) {
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
}
