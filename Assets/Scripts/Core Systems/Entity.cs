using System;
using UnityEngine;

/// <summary>
/// Class that PlayerHealth and EnemyHealth inherit from. Stores a HealthOwner which holds
/// the Entity's team, and max hp
/// </summary>
public abstract class Entity : MonoBehaviour
{
    [NonSerialized] public HealthOwner healthController;
    // will add more things in the future
}