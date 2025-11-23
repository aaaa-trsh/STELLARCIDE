using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static readonly GameManager Instance = new();

    public GameObject Player;
    public ProjectileManager ProjectileManager;

}