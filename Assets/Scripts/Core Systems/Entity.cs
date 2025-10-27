using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public HealthController healthController;
    public HealthController.Team team => healthController.team;

}