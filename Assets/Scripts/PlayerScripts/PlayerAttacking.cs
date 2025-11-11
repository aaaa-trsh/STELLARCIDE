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
            travelSpeed: 10,
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + 1, transform.position.x), new Vector3(1.5f, 1.5f, 1.5f));
    }
    

    void SwapBaseAttack(bool isShip)
    {
        if (isShip)
        {
            BaseAttack = new Shoot(self,
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 1f,
                travelSpeed: 10,
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