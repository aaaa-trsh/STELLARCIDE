using System;
using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class PlayerShipMovement : MonoBehaviour
{
    [SerializeField] private float thrustCamSpeed;
    [SerializeField] private bool twinStick;
    
    private float thrustMinDist = 1;
    private float thrustMaxDist = 4;

    private float initialCamTargetDist = 0;
    private ShipMovement shipMovement;
    private ClampedFollower camTarget;

    void Start() {
        camTarget = GetComponentInChildren<ClampedFollower>();
        initialCamTargetDist = camTarget.maxDistance;

        shipMovement = GetComponent<ShipMovement>();
    }

    void Update() {
        Vector3 cursor = CursorUtil.GetCursorPosition();
        Vector3 cursorOffset = cursor - transform.position;
        
        // turn towards cursor
        if (cursorOffset.magnitude > thrustMinDist) {
            float angle = Mathf.Atan2(cursorOffset.y, cursorOffset.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        // set wish dir
        Vector2 thrustInput;
        if (twinStick) {
            thrustInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        } else {
            if (cursorOffset.magnitude > thrustMinDist) {
                float t = Mathf.Clamp01((cursorOffset.magnitude - thrustMinDist) / (thrustMaxDist - thrustMinDist));
                thrustInput = transform.right * (Input.GetKey(KeyCode.Space) ? 1 : 0) * t;
            } else {
                thrustInput = Vector2.zero;
            }
        }

        shipMovement.SetWishDirection(thrustInput);
        
        // move camera
        if (thrustInput.magnitude > 0) {   
            camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, initialCamTargetDist, Time.deltaTime * thrustCamSpeed);
        } else {
            camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, 0, Time.deltaTime * thrustCamSpeed);
        }
    }

}
