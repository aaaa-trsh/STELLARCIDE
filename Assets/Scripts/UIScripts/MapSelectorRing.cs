using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class MapSelectorRing : MonoBehaviour
{
    public float ringSize = 1f;
    public float smoothTime = 0.5f;
    public float smoothVelocity = 0.0f;
    [SerializeField] private float maxSize, minSize;
    bool backwards = false;

    private void Start()
    {
        StartCoroutine(LerpFunction(maxSize, smoothTime));
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(ringSize, ringSize, ringSize);
        transform.Rotate(0, 0, 0.1f, Space.Self);
    }
    
    IEnumerator LerpFunction(float endValue, float duration)
    {
        float time = 0;
        float startValue = ringSize;
        Vector3 startScale = transform.localScale;

        while (time < duration)
        {
            ringSize = Mathf.Lerp(startValue, endValue, time / duration);
            transform.localScale = startScale * ringSize;
            time += Time.deltaTime;
            yield return null;
        }
        
        ringSize = endValue;
        backwards = !backwards;
        if (!backwards)
        {
            // ringSize = Mathf.SmoothDamp(ringSize, maxSize, ref smoothVelocity, smoothTime);
            StartCoroutine(LerpFunction(maxSize, smoothTime));
        }
        else //ringSize = Mathf.SmoothDamp(ringSize, minSize, ref smoothVelocity, smoothTime);
            StartCoroutine(LerpFunction(minSize, smoothTime));
    }
    
}
