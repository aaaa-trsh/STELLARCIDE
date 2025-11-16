using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Variables")] [SerializeField]
    private float speed = 10f;

    [SerializeField] private float turnSpeed = 20f;
    [SerializeField] private float maxVelocity = 10f;
    
    [Tooltip("Determines how many times less Mech speed is compared to Max Velocity")]
    [SerializeField] private float mechMovementReductionCoefficient = 2f;
    
    [Header("References")]
    [SerializeField] private Camera playerCam;
    
    private Rigidbody2D playerRb;
    
    public bool ship = true;
    private bool _moving = false;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // if ship form
        if (ship) HandleMovementShip();
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
        
        if (Input.GetKey(KeyCode.Space))
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

        _moving = false;

        Vector2 velocity = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector2(0, maxVelocity / mechMovementReductionCoefficient);
            _moving = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector2(0, -maxVelocity / mechMovementReductionCoefficient);
            _moving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector2(maxVelocity / mechMovementReductionCoefficient, 0);
            _moving = true;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector2(-maxVelocity / mechMovementReductionCoefficient, 0);
            _moving = true;
        }
        
        playerRb.linearVelocity = velocity;
        
        if (!_moving)
        {
            playerRb.linearVelocity = Vector2.zero;
        }
        
    }

    public void SwapForm()
    {
        ship = !ship;
        EventBus.Instance.ChangeForm(ship);
    }

    public void StopVelocity()
    {
        playerRb.linearVelocity = Vector2.zero;
    }
}
