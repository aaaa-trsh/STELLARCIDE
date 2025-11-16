using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private GameObject shipSpriteTEMP;
    private PlayerController playerController;

    void Start() {
        playerController = GetComponent<PlayerController>();
    }

    void Update() {
        PlayerMode mode = playerController.GetPlayerMode();
        
        switch (mode) {
            case PlayerMode.MECH:
                shipSpriteTEMP.SetActive(false);
                break;

            case PlayerMode.SHIP:
            default:
                shipSpriteTEMP.SetActive(true);
                break;
        }
    }
}
