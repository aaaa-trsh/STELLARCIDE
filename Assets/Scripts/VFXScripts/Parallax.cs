using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Parallax : MonoBehaviour
{
    private Material mat;
    private float distance;
    // position that the change is based off
    private Vector2 changeSpot;
    
    // get player transform
    public Transform player;
    
    // speed of change
    [Range(0f, 0.005f)]
    public float speed = 0.005f;
    
    void Start()
    {
        if (player == null)
            player =  GameObject.FindGameObjectWithTag("Player").transform;
        changeSpot = transform.position;
        mat =  GetComponent<Renderer>().material;
    }


    void Update()
    {
        if (player == null)
            return;
        
        // find the difference between the center of the background and the player
        Vector2 change = new Vector2(player.position.x - changeSpot.x, player.position.y - changeSpot.y);
        
        mat.SetTextureOffset("_MainTex", change*speed); 
    }
}
