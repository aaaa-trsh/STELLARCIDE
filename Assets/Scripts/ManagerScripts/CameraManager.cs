using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(UITest.gameActive)
            gameObject.GetComponent<CinemachineBrain>().enabled = false;
        else
        {
            gameObject.GetComponent<CinemachineBrain>().enabled = true;
        }
        
    }
}
