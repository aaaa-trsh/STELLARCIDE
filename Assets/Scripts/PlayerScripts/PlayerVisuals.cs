using UnityEngine;

[RequireComponent(typeof(ShipMovement))]
public class PlayerVisuals : MonoBehaviour
{
    [SerializeField] private GameObject shipSprite;
    [SerializeField]  private GameObject mechSprite;
    private PlayerController playerController;

    void Awake()
    {
        playerController = GetComponent<PlayerController>();
        EventBus.Instance.OnFormChange += SwapSprites;
    }
    private void OnDestroy()
    {
        if (EventBus.Instance != null)
        {
            EventBus.Instance.OnFormChange -= SwapSprites;
        }
    }

    void Start() 
    {
        shipSprite.SetActive(true);
        mechSprite.SetActive(false);
    }

    void SwapSprites(PlayerMode newMode)
    {
        switch (newMode)
        {
            case PlayerMode.SHIP:
                shipSprite.SetActive(true);
                mechSprite.SetActive(false);
                break;
            case PlayerMode.MECH:
                shipSprite.SetActive(false);
                mechSprite.SetActive(true);
                break;
        }
    }
}
