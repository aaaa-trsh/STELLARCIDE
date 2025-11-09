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
        BaseAttack = new Shoot(gameObject, new Damage(10, Damage.Type.PHYSICAL), 1f, 10, 2, true);
        EventBus.Instance.OnFormChange += (isShip) =>
        {
            if (isShip)
                BaseAttack = new Shoot(gameObject, new Damage(10, Damage.Type.PHYSICAL), 1f, 10, 2, true);
            else
                BaseAttack = new Punch(gameObject, new Damage(10, Damage.Type.PHYSICAL), 1f);
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (BaseAttack.IsReady()) // check if in cooldown
                CoroutineManager.Instance.Run(BaseAttack.Execute(gameObject.transform.position, gameObject.transform.right));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y+1, transform.position.x), new Vector3(1.5f, 1.5f, 1.5f));
    }
}