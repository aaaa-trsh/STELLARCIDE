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
    
    [Header("References")]
    [SerializeField] private Camera playerCam;
    
    private Rigidbody2D playerRb;

    private float horizontalMovement;
    private float verticalMovement;
    private bool ship = true;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        // if ship form
        if (ship) HandleMovementShip();
        // if mech form
        else HandleMovementMech();
        
        // Rotate to Mouse
        Vector2 direction = playerCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    private void HandleMovementShip()
    {
        if (Input.GetKey(KeyCode.W))
            playerRb.AddForce(transform.right * speed);
           
        else
        {
            playerRb.linearVelocity = playerRb.linearVelocity / 1.03f;
        }
        
    }

    private void HandleMovementMech()
    {
        playerRb.linearVelocity = new Vector2(horizontalMovement*speed, verticalMovement * speed);
    }

    public void SwapForm()
    {
        ship = !ship;
    }

    public void StopVelocity()
    {
        playerRb.linearVelocity = Vector2.zero;
    }
}
