using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Start()
    {
        Instance = this;
    }

    public GameObject Player;
    public ProjectileManager ProjectileManager;

}