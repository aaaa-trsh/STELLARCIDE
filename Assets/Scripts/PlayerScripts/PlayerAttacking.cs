using System;
using UnityEditor.EditorTools;
using UnityEngine;
/// <summary>
/// Attach this to the player
/// </summary>
public class PlayerAttacking : MonoBehaviour
{
    [NonSerialized] public Attack BaseAttack;

    void Start()
    {
        BaseAttack = new Shoot(gameObject, Attack.Type.RANGED, new Damage(10, Damage.Type.PHYSICAL), 0.02f, 10, 2, true);
        EventBus.Instance.OnFormChange += (isShip) =>
        {
            if (isShip)
                BaseAttack = new Shoot(gameObject, Attack.Type.RANGED, new Damage(10, Damage.Type.PHYSICAL), 0.02f, 10, 2, true);
            else
                BaseAttack = new Punch(gameObject, Attack.Type.MELEE, new Damage(10, Damage.Type.PHYSICAL), 0.02f);
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BaseAttack.IsReady()) // check if in cooldown
                Debug.Log("reached playerAttacking.cs");
                CoroutineManager.Instance.Run(BaseAttack.Execute(gameObject.transform.position, gameObject.transform.right));
        }
    }
}