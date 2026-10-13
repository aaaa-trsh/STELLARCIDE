using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private float shipTurnSpeed = 30f;
    
    [Header("Mech Settings")]
    [SerializeField] private float mechCameraDist = 2;
    [SerializeField] private LayerMask mechTransitionLayer;
    [SerializeField] private float mechTurnSpeed = 100f;
    
    [Header("Slide Settings")]
    [SerializeField] private float slideSpeed = 15;
    [SerializeField] private float slideLength = 0.5f;

    [NonSerialized] public bool inputEnabled;

    private PlayerMode currentMode = PlayerMode.SHIP;
    
    private float targetCameraDistance;
    private float initialCamTargetDist = 0;
    private ClampedFollower camTarget;

    private ShipMovement shipMovement;
    private MechMovement mechMovement;
    private PlayerControls inputActions;

    private Vector3 cursor;
    private Vector3 cursorOffset;
    
    private Rigidbody2D rb;
    private Animator animator;
    private CapsuleCollider2D _collider;

    private void Awake()
    {
        GameManager.Instance.Player = gameObject;
        GameManager.Instance.GameActive = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        inputActions = new PlayerControls();
        
        shipMovement = GetComponent<ShipMovement>();
        shipMovement.enabled = true;
        mechMovement = GetComponent<MechMovement>();
        mechMovement.enabled = false;
        _collider = GetComponent<CapsuleCollider2D>();

        ToggleControls(true);
    }

    void OnEnable()
    {
        inputActions.Enable();
        _collider.enabled = true;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void OnDestroy()
    {
        inputActions.Disable();
    }

    void Start() {
        camTarget = GetComponentInChildren<ClampedFollower>();
        initialCamTargetDist = camTarget.maxDistance;
        targetCameraDistance = initialCamTargetDist;
    }

    void Update() {

        if (!GameManager.Instance.GameActive)
        {
            return;
        }
        
        // update cursor
        cursor = CursorUtil.GetCursorPosition();
        cursorOffset = cursor - transform.position;

        
        // handle movement modes
        if (currentMode == PlayerMode.SHIP) {
            HandleShipControls();
        } else if (currentMode == PlayerMode.MECH) {
            HandleMechControls();
        }

        // move the camera target
        camTarget.maxDistance = Mathf.Lerp(camTarget.maxDistance, targetCameraDistance, Time.deltaTime * thrustCamSpeed);
    }

    void HandleShipControls() {
        if (cursorOffset.magnitude > shipMinCameraDist) {
            TurnToCursor(shipTurnSpeed);
        }

        // input based off thrust
        Vector2 wishDir;
        if (cursorOffset.magnitude > shipMinCameraDist) {
            float t = Mathf.Clamp01((cursorOffset.magnitude - shipMinCameraDist) / (shipMaxCameraDist - shipMinCameraDist));
            wishDir = transform.right * (inputActions.Gameplay.Thrust.IsPressed() ? 1 : 0) * t;
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
        
        TurnToCursor(mechTurnSpeed);

        // directional input
        Vector2 wishDir = inputActions.Gameplay.Move.ReadValue<Vector2>().normalized;
        
            mechMovement.SetWishDirection(wishDir);

        targetCameraDistance = mechCameraDist;
    }

    void TurnToCursor(float turnSpeed) {
        float angle = Mathf.Atan2(cursorOffset.y, cursorOffset.x) * Mathf.Rad2Deg;

        // Create target rotation
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Constant Turn Rate
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // very simple state transitions
    void OnTriggerEnter2D(Collider2D other) {
        if (gameObject == null ) {return;}

        if ((mechTransitionLayer & (1 << other.gameObject.layer)) != 0) {
            FlyIn();
            currentMode = PlayerMode.MECH;
            EventBus.Instance.ChangeForm(currentMode);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (gameObject == null ) {return;}

        if ((mechTransitionLayer & (1 << other.gameObject.layer)) != 0) {
            FlyOut();
            currentMode = PlayerMode.SHIP;
            EventBus.Instance.ChangeForm(currentMode);
        }
    }

    void FlyIn()
    {
        shipMovement.enabled = false;
        mechMovement.enabled = true;

        if (!gameObject.activeInHierarchy) { return; }
        StartCoroutine(LockControls(slideLength));
        rb.AddForce(transform.right * slideSpeed, ForceMode2D.Impulse);
        // transformation animation (ship to mech)
        
        // sound effect
    }

    void FlyOut()
    {
        shipMovement.enabled = true;
        mechMovement.enabled = false;
        if (!gameObject.activeInHierarchy) { return; }
        StartCoroutine(LockControls(slideLength));
        // addforce ( direction of movement * speed)
        Vector2 wishDir = inputActions.Gameplay.Move.ReadValue<Vector2>().normalized;
        if (wishDir.magnitude > 0.001f)
        {
            rb.AddForce(wishDir * (slideSpeed/2), ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(transform.right * (slideSpeed/2), ForceMode2D.Impulse);
        }
        
        // transformation animation (mech to ship)

        // sound effect
    }

    /// <summary>
    /// Temporarily disables player input for a duration
    /// </summary>
    public IEnumerator LockControls(float duration)
    {
        ToggleControls(false);
        yield return new WaitForSeconds(duration);
        ToggleControls(true);
    }

    /// <summary>
    /// Enable and disable player inputs until called again
    /// </summary>
    public void ToggleControls(bool enabled)
    {
        if (enabled)
        {
            inputActions.Enable();
        }
        else
        {
            inputActions.Disable();
        }
        inputEnabled = enabled;
    }
    
    public PlayerMode GetPlayerMode() {
        return currentMode;
    }
}
