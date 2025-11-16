using UnityEngine;

public enum PlayerMode {
    SHIP, MECH
}

[RequireComponent(typeof(ShipMovement))]
public class PlayerController : MonoBehaviour
{
    [Header("Ship Settings")]
    [SerializeField] private float thrustCamSpeed;
    [SerializeField] private float shipMinCameraDist = 1;
    [SerializeField] private float shipMaxCameraDist = 4;
    
    [Header("Mech Settings")]
    [SerializeField] private float mechCameraDist = 2;
    [SerializeField] private LayerMask mechTransitionLayer;

    private PlayerMode currentMode = PlayerMode.SHIP;
    
    private float targetCameraDistance;
    private float initialCamTargetDist = 0;
    private ClampedFollower camTarget;

    private ShipMovement shipMovement;
    private MechMovement mechMovement;

    private Vector3 cursor;
    private Vector3 cursorOffset;

    void Start() {
        camTarget = GetComponentInChildren<ClampedFollower>();
        initialCamTargetDist = camTarget.maxDistance;
        targetCameraDistance = initialCamTargetDist;

        shipMovement = GetComponent<ShipMovement>();
        mechMovement = GetComponent<MechMovement>();
    }

    void Update() {
        // update cursor
        cursor = CursorUtil.GetCursorPosition();
        cursorOffset = cursor - transform.position;

        // handle movement modes
        if (currentMode == PlayerMode.SHIP) {
            shipMovement.enabled = true;
            mechMovement.enabled = false;
            HandleShipControls();
        } else if (currentMode == PlayerMode.MECH) {
            shipMovement.enabled = false;
            mechMovement.enabled = true;
            HandleMechControls();
        }

        // move the camera target
        camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, targetCameraDistance, Time.deltaTime * thrustCamSpeed);
    }

    void HandleShipControls() {
        if (cursorOffset.magnitude > shipMinCameraDist) {
            TurnToCursor();
        }

        // input based off thrust
        Vector2 wishDir;
        if (cursorOffset.magnitude > shipMinCameraDist) {
            float t = Mathf.Clamp01((cursorOffset.magnitude - shipMinCameraDist) / (shipMaxCameraDist - shipMinCameraDist));
            wishDir = transform.right * (Input.GetKey(KeyCode.Space) ? 1 : 0) * t;
        } else {
            wishDir = Vector2.zero;
        }

        shipMovement.SetWishDirection(wishDir);

        // move camera
        if (wishDir.magnitude > 0) {   
            targetCameraDistance = initialCamTargetDist;
        } else {
            targetCameraDistance = 0;
        }

    }

    void HandleMechControls() {
        TurnToCursor();

        // directional input
        Vector2 wishDir = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical")
        ).normalized;

        mechMovement.SetWishDirection(wishDir);
        targetCameraDistance = mechCameraDist;
    }

    void TurnToCursor() {
        float angle = Mathf.Atan2(cursorOffset.y, cursorOffset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // very simple state transitions
    void OnTriggerEnter2D(Collider2D other) {
        if ((mechTransitionLayer & (1 << other.gameObject.layer)) != 0) {
            currentMode = PlayerMode.MECH;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if ((mechTransitionLayer & (1 << other.gameObject.layer)) != 0) {
            currentMode = PlayerMode.SHIP;
        }
    }

    public PlayerMode GetPlayerMode() {
        return currentMode;
    }
}
