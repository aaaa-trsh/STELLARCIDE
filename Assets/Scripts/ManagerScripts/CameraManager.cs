using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<CinemachineBrain>().enabled = UITest.gameActive ? true : false;
    }
}
