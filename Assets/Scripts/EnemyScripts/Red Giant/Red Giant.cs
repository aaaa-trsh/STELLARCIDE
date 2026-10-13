using System.Collections.Generic;
using UnityEngine;

public class RedGiant : MonoBehaviour
{

    [Header("Body Info")]
    public int SegmentCount = 0;
    public Head HeadPrefab;
    public Body BodyPrefab;
    public Tail TailPrefab;
    // Hold references to each part
    private Head HeadSegment;
    private List<Body> BodySegments = new List<Body>();
    private Tail TailSegment;
    private int bodySpacing = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //// Create head
        //GameObject head = Instantiate(HeadPrefab, transform.position, transform.rotation, transform);
        //head.GetComponent<MarkerManager>().ClearMarkerList();
        //bodyParts.Add(head);

        //CreateBody();
        HeadSegment = Instantiate(HeadPrefab, transform.position, transform.rotation, transform);
        Transform prevTransform = HeadSegment.transform;
        for (int i = 1; i < SegmentCount + 1; i++)
        {
            Body body = Instantiate(BodyPrefab, new Vector2(transform.position.x, transform.position.y + (i * bodySpacing)), 
                transform.rotation, transform);
            body.target = prevTransform;
            BodySegments.Add(body);
            prevTransform = body.transform;
        }
        TailSegment = Instantiate(TailPrefab, new Vector2(transform.position.x, transform.position.y + ((SegmentCount + 1) * bodySpacing)), 
            transform.rotation, transform);
        TailSegment.target = prevTransform;
    }
}
