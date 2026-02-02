using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(ShipMovement))]
public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private GameObject shipSprite;
    [SerializeField]  private GameObject mechSprite;
    private PlayerController playerController;

    void Start() {
        playerController = GetComponent<PlayerController>();
        shipSprite.SetActive(true);
        mechSprite.SetActive(false);
    }

    void Update() {
        PlayerMode mode = playerController.GetPlayerMode();
        
        switch (mode) {
            case PlayerMode.MECH:
                shipSprite.SetActive(false);
                mechSprite.SetActive(true);
                break;

            case PlayerMode.SHIP:
            default:
                shipSprite.SetActive(true);
                mechSprite.SetActive(false);
                break;
        }
    }
}
