using System;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [NonSerialized] public HealthOwner healthController;
    public HealthOwner.Team team => healthController.team;
}