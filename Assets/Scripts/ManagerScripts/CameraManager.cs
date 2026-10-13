using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    CinemachineBrain cb;
    void Awake()
    {
        cb = GetComponent<CinemachineBrain>();
    }
    
    void Update()
    {
        cb.enabled = GameManager.Instance.GameActive ? true : false;
    }
}
