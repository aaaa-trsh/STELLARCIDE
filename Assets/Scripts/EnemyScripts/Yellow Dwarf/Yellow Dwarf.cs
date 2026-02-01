using System.Collections;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class YellowDwarf : MonoBehaviour
{
    // TO-DO: Add different states (moving towards player and actually attacking)
    // When moving towards player, make it so it is not in a straight line towards the player (add some variation to the side)
    // When attacking the player, it attacks in a straight line)
    public Transform Player;
    private Vector3 Target;

    public Attack DashAttack;
    private GameObject self;

    private float DashDistance = 8.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        self = gameObject;
        DashAttack = new Dash(self,
            damage: new Damage(10, Damage.Type.PHYSICAL),
            cooldown: 1f,
            travelSpeed: 0.25f,
            lifetime: 1f
            );
    }

    public static Vector2 rotate(Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.position - transform.position;
        Vector3 targetPosition = Player.position;

        if ((direction.magnitude > 5) && !DirectionSet)
        {
            direction = rotate(direction, Random.Range(-1.0f, 1.0f) * Mathf.Rad2Deg);
            targetPosition = transform.position + direction;
            DirectionSet = true;
        }
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 90f;
        targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        bool facingTarget = Quaternion.Angle(transform.rotation, targetRotation) < 0.5f;

        if (!facingTarget && !IsRotating)
        {
            RotateToTarget();
            return;
        }

        if (facingTarget && DashAttack.IsReady() && !IsRotating)
        {
            Vector3 dashDirection = (targetPosition - transform.position).normalized * DashDistance;

            CoroutineManager.Instance.Run(DashAttack.Execute(transform.position, transform.position + dashDirection));

            DirectionSet = false;
        }
    }

    private bool DirectionSet = false;
    private bool IsRotating = false;
    private Quaternion targetRotation;

    private void RotateToTarget()
    {
        if (!IsRotating)
        {
            StartCoroutine(RotateToTargetCoroutine());
        }
    }
    private IEnumerator RotateToTargetCoroutine()
    {
        IsRotating = true;

        float t = 0.0f;
        float duration = 0.5f;

        Quaternion startRotation = transform.rotation;
        while (t < 1.0f)
        {
            t += Time.deltaTime / duration;
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
        IsRotating = false;

    }
}
