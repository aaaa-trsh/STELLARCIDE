using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Class that PlayerHealth and EnemyHealth inherit from. Stores a HealthOwner which holds
/// the Entity's team, and max hp
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [NonSerialized] public HealthOwner healthController;
    public StateController sc;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin">Position of the source of knockback</param>
    /// <param name="strength">Distance to be knocked back; min:0.5, max:inf</param>
    /// <returns></returns>
    public IEnumerator KnockBack(Vector3 origin, float strength)
    {
        if (sc)
        {
            sc.locked = true;
        }

        float elapsedTime = 0;
        Vector3 directionVector = (transform.position - origin).normalized * strength;

        while (elapsedTime < 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, 
                origin + directionVector, 
                elapsedTime / 0.5f
            );
            elapsedTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
        transform.position = origin + directionVector;

        if (sc)
        {
            sc.locked = false;
        }

        yield return new WaitForEndOfFrame();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}