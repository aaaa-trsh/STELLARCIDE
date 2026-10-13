using UnityEngine;

public class Body : MonoBehaviour
{
    public Transform target;
    public float followDistance = 4.0f;
    public float followSpeed = 5f;

    public bool freeze = false;

    void Update()
    {
        if (!target || freeze) return;


        float actualDistance = Vector2.Distance(transform.position, target.position);

        var followToCurrent = (transform.position - target.position).normalized;
        followToCurrent.Scale(new Vector2(followDistance, followDistance));
        transform.position = Vector2.MoveTowards(transform.position, followToCurrent + target.position, followSpeed * Time.deltaTime);


        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, followSpeed * 10 * Time.deltaTime);
    }
}
