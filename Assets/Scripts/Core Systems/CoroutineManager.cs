using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    void Start()
    {
        Instance = this;
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}