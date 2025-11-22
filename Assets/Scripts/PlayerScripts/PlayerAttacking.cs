using System;
using UnityEngine;
/// <summary>
/// Controller script for player attacks 
/// </summary>
public class PlayerAttacking : MonoBehaviour
{
    [NonSerialized] public Attack BaseAttack;
    private GameObject self;


    void Start()
    {
        // seems a bit redundant but for some reason this solves a bug
        self = gameObject;
        // start off with a Shoot attack
        BaseAttack = new Shoot(self,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 30,
            lifetime: 2,
            piercing: true
        );
        // this script now observes whenever the player changes forms and switches attacks accordingly
        EventBus.Instance.OnFormChange += (isShip) => SwapBaseAttack(isShip);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // this is how you actually attack
            if (BaseAttack.IsReady()) // check if in cooldown
                CoroutineManager.Instance.Run(BaseAttack.Execute(self.transform.position, self.transform.right));
        }
    }


    // punch hitbox vizualizer 
    void OnDrawGizmos()
    {
        // punch hitbox holy shit
        Gizmos.color = Color.red;
        Vector3[] points = new Vector3[4]
        {
          transform.localPosition + (2 * transform.right) + (0.75f * transform.up), // top right
          transform.localPosition + (2 * transform.right) - (0.75f * transform.up), // top left
          transform.localPosition - (0.75f * transform.up), // bot left
          transform.localPosition + (0.75f * transform.up), // bot right
        };
        Vector3[] faces = new Vector3[8]
        {
            points[0], points[1],
            points[1], points[2],
            points[2], points[3],
            points[3], points[0],
        };
        Gizmos.DrawLineList(faces);

        // dash prediction
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y, transform.position.z), 
            new Vector3(1, 10, 1)
        );
    }
    

    void SwapBaseAttack(bool isShip)
    {
        if (isShip)
        {
            BaseAttack = new Shoot(self,
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 1f,
                travelSpeed: 30,
                lifetime: 2,
                piercing: true
            ); 
        }
        else
        {
            BaseAttack = new Punch(self,
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 1f
            );
        }  
    }
}