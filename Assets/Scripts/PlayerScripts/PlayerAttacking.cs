using System;
using Unity.Cinemachine;
using UnityEngine;
/// <summary>
/// Controller script for player attacks 
/// </summary>
public class PlayerAttacking : MonoBehaviour
{
    [NonSerialized] public Attack BaseAttack;
    [NonSerialized] public Attack SecondaryAttack;
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
        SecondaryAttack = new Dash(self, 
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            lifetime: 1f
        );
        // this script now observes whenever the player changes forms and switches attacks accordingly
        EventBus.Instance.OnFormChange += (isShip) => SwapAttacks(isShip);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // this is how you actually attack
            if (BaseAttack.IsReady()) // check if in cooldown
                CoroutineManager.Instance.Run(BaseAttack.Execute(self.transform.position, self.transform.right));
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = transform.position.z;
            
            try
            {
            if (SecondaryAttack.IsReady())
                CoroutineManager.Instance.Run(SecondaryAttack.Execute(self.transform.position, mouseWorldPos));
            } catch{}
        }
    }


    // punch hitbox vizualizer 
    void OnDrawGizmos()
    {
        // punch hitbox holy shit
        Gizmos.color = Color.red;
        Vector3[] points = new Vector3[4]
        {
          transform.localPosition + (3 * transform.right) + (1.5f * transform.up), // top right
          transform.localPosition + (3 * transform.right) - (1.5f * transform.up), // top left
          transform.localPosition - (1.5f * transform.up), // bot left
          transform.localPosition + (1.5f * transform.up), // bot right
        };
        Vector3[] faces = new Vector3[8]
        {
            points[0], points[1],
            points[1], points[2],
            points[2], points[3],
            points[3], points[0],
        };
        Gizmos.DrawLineList(faces);
    }
    

    void SwapAttacks(bool isShip)
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
            SecondaryAttack = null;
        }
        else
        {
            BaseAttack = new Punch(self,
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 0.5f
            );
            SecondaryAttack = new Dash(self, 
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 1f,
                lifetime: 1f
            );
        }  
    }
}