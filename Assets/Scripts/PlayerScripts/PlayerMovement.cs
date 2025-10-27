using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float turnSpeed =  20f;
    [SerializeField] private float maxVelocity = 10f;
    
    [Header("References")]
    [SerializeField] private Camera playerCam;
    
    private Rigidbody2D playerRb;
    
    private bool _ship = true;
    private bool _moving = false;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // if ship form
        if (_ship) HandleMovementShip();
        // if mech form
        else HandleMovementMech();
    }

    private void HandleMovementShip()
    {
        // Rotate toward Mouse over time
        Vector2 direction = (Vector2)playerCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Create target rotation
        Quaternion targetRotation = Quaternion.AngleAxis(targetAngle, Vector3.forward);

        // Constant Turn Rate
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        
        if (Input.GetKey(KeyCode.W))
        {
            playerRb.AddForce(transform.right * speed);
            
            // stabilize sideways drift
            Vector2 forward = transform.right; // The "forward" direction in 2D
            Vector2 velocity = playerRb.linearVelocity;

            // Project current velocity onto forward direction
            Vector2 forwardVel = Vector2.Dot(velocity, forward) * forward;

            // Subtract forwardVel from velocity to get sideways component
            Vector2 sidewaysVel = velocity - forwardVel;

            Vector2 desiredVel = forwardVel + sidewaysVel * 0.1f;
            playerRb.linearVelocity = Vector2.Lerp(playerRb.linearVelocity, desiredVel, Time.deltaTime * 5f);
        }
        else
        {
            playerRb.linearVelocity /= 1.03f;
        }

        if (playerRb.linearVelocity.magnitude > maxVelocity)
        {
            playerRb.linearVelocity = playerRb.linearVelocity.normalized * maxVelocity;
        }
    }

    private void HandleMovementMech()
    {
        // Rotate to Mouse
        Vector2 direction = playerCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        
        Vector2 forward = transform.right; // forward
        Vector2 velocity = new Vector2(maxVelocity/2, 0);

        _moving = false;
        
        if (Input.GetKey(KeyCode.W))
        {
            direction = (Vector2)playerCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            direction.Normalize();
            // move in direction by speed
            playerRb.linearVelocity = direction * speed/2; // move in direction by speed
            // now moving
            _moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction = (Vector2)playerCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            direction.Normalize();
            playerRb.linearVelocity = -direction * speed/2; // move in opposite direction by speed
            // now moving
            _moving = true;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            direction = (Vector2)playerCam.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            direction.Normalize(); 

            // Get the tangent (perpendicular) direction
            Vector2 tangent = new Vector2(-direction.y, direction.x);

            // If moving right instead of left, flip tangent
            if (Input.GetKey(KeyCode.D))
                tangent = -tangent;

            // Apply movement
            playerRb.linearVelocity = tangent * speed;

            _moving = true;
        }

        if (!_moving)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
        
    }

    public void SwapForm()
    {
        _ship = !_ship;
    }

    public void StopVelocity()
    {
        playerRb.linearVelocity = Vector2.zero;
    }
}
