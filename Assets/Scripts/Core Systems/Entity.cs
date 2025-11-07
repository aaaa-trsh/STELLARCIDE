using System;
using UnityEngine;

/// <summary>
/// Class that PlayerHealth and EnemyHealth inherit from
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [NonSerialized] public HealthOwner healthController;
}