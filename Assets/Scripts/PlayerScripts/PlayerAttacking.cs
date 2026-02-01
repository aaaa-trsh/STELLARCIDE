using System;
using UnityEngine;
using Unity.VisualScripting;

/* TODO:
++ store inventory of attacks
-- i.e. AttackInventory[] & AttackPool[]
++ take attack data from attacks.json
++ create attacks using data
*/

/// <summary>
/// Controller script for player attacks 
/// </summary>
public class PlayerAttacking : MonoBehaviour
{
    [NonSerialized] public Attack BaseAttack;
    [NonSerialized] public Attack SecondaryAttack;
    // private GameObject self;

    void Awake()
    {
        // seems a bit redundant but for some reason this solves a bug
        // self = gameObject;
        // start off with a Shoot attack
        BaseAttack = new Shoot(gameObject,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 30,
            lifetime: 2,
            piercing: true
        );
    }

    void Start()
    {
        // this script now observes whenever the player changes forms and switches attacks accordingly
        EventBus.Instance.OnFormChange += (isShip) => SwapAttacks(isShip);
    }


    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // this is how you actually attack
            if (BaseAttack.IsReady()) // check if in cooldown
            {
                CoroutineManager.Instance.Run(BaseAttack.Execute(gameObject.transform.position, gameObject.transform.right));
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (!SecondaryAttack.IsUnityNull())
            {
                if (SecondaryAttack.IsReady())
                {
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPos.z = transform.position.z;
                    CoroutineManager.Instance.Run(SecondaryAttack.Execute(gameObject.transform.position, mouseWorldPos));
                }
            }
        }
    }

    void SwapAttacks(bool isShip)
    {
        if (isShip)
        {
            BaseAttack = new Shoot(gameObject,
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
            BaseAttack = new Punch(gameObject,
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 0.5f
            );

            SecondaryAttack = new Dash(gameObject, 
                damage: new Damage(10, Damage.Type.PHYSICAL),
                cooldown: 1f,
                travelSpeed:0.25f, // looks like the max value before there is a pause after a dash
                lifetime: 1f
            );
        }  
    }
}