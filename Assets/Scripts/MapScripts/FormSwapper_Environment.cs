using System;
using UnityEngine;
using UnityEngine.Serialization;

public class FormSwapper_Environment : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        { 
            Debug.Log("Passed check");
            player.SwapForm();
            player.StopVelocity();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement player))
        {
            player.SwapForm(); 
        }
    }
}
