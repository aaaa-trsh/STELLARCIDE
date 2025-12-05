using UnityEngine;

public class GameManager
{
    static GameManager thisInstance;

    public static GameManager Instance
    {
        get { return thisInstance ??= new GameManager(); }
    }

    public GameObject Player;
    public ProjectileManager ProjectileManager;

}