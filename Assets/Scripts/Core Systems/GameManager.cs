using Unity.VisualScripting;
using UnityEngine;

public class GameManager
{
    public static readonly GameManager Instance = new();

    public GameObject Player;
    public ProjectileManager ProjectileManager;

}