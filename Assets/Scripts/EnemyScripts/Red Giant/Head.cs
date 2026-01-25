using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Head : MonoBehaviour
{
    public Transform Player;
    private float DistanceToPlayer;

    [Header("Body Info")]
    public int SegmentCount = 0;
    public Body BodyPrefab;
    public Tail TailPrefab;
    // Hold references to each part
    private List<Body> BodySegments = new List<Body>();
    private Tail TailSegment;

    [Header("Enemy Properties")]
    private float MoveSpeed = 2.0f;
    private bool IsAttacking = false;

    [Header("Attack Properties")]
    public float windupDistance = 1.2f;
    public float windupTime = 0.25f;
    public float lungeDistance = 3f;
    public float lungeTime = 0.15f;
    public float attackCooldown = 5f;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        //Player = FindFirstObjectByType<PlayerController>().transform; // Find transform of player

        Transform prevTransform = transform;
        for (int i = 1; i < SegmentCount + 1; i++)
        {
            Body body = Instantiate(BodyPrefab, new Vector2(transform.position.x, transform.position.y + (i)), transform.rotation);
            body.target = prevTransform;
            BodySegments.Add(body);
            prevTransform = body.transform;
        }
        TailSegment = Instantiate(TailPrefab, new Vector2(transform.position.x, transform.position.y + (SegmentCount + 1)), transform.rotation);
        TailSegment.target = prevTransform;
    }

    private void Update()
    {
        if (!Player || IsAttacking) { return; } // If no player or is in the middle of attacking return
        DistanceToPlayer = (Player.position - transform.position).magnitude;
        if (!IsAttacking && (DistanceToPlayer < 3)) {
            StartAttack();
        }
        Move();
    }

    private void StartAttack()
    {
        if (!IsAttacking)
        {
            StartCoroutine(LungeAttack());
        }
    }

    private GameObject self;
    private IEnumerator LungeAttack()
    {
        FreezeSegments(true);
        IsAttacking = true;
        Vector3 startPos = transform.position;
        Vector3 backPos = startPos - transform.up * windupDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / windupTime;
            transform.position = Vector3.Lerp(startPos, backPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        Vector3 lungePos = backPos + transform.up * lungeDistance;
        t = 0f;

        while (t < 1f)
        {
            // Attack stuff would be set here
            t += Time.deltaTime / lungeTime;
            transform.position = Vector3.Lerp(backPos, lungePos, t);
            yield return null;
        }

        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / lungeTime;
            transform.position = Vector3.Lerp(lungePos, startPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(attackCooldown);
        FreezeSegments(false);
        IsAttacking = false;
    }

    private void FreezeSegments(bool f)
    {
        foreach (Body b in BodySegments)
        {
            b.freeze = f;
        }
        TailSegment.freeze = f;
    }

    private void Move()
    {
        transform.up = (Player.position - transform.position).normalized;
        transform.position += transform.up * Time.deltaTime * MoveSpeed;
    }
    /*
    [Header("Attack")]
    public float windupDistance = 1.2f;
    public float windupTime = 0.25f;
    public float lungeDistance = 3f;
    public float lungeTime = 0.15f;
    public float attackCooldown = 1f;

    bool isAttacking;

    public float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 2 && !isAttacking)
        {
            StartAttack();
            print("Starting attack");
        }
        if (isAttacking)
        {
            return;
        }
        Move();
    }

    void Move()
    {

        //transform.up = (target.position - transform.position).normalized;
        transform.up = (GetMouseWorldPosition() - (Vector2)transform.position).normalized;

        transform.position += transform.up * Time.deltaTime * moveSpeed;
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 world = Camera.main.ScreenToWorldPoint(mouseScreen);
        return new Vector2(world.x, world.y);
    }

    public void StartAttack()
    {
        if (!isAttacking)
        {
            StartCoroutine(LungeAttack());
        }
    }

    IEnumerator LungeAttack()
    {
        isAttacking = true;
        Vector3 startPos = transform.position;
        Vector3 backPos = startPos - transform.up * windupDistance;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / windupTime;
            transform.position = Vector3.Lerp(startPos, backPos, t);
            yield return null;
        }

        // Small anticipation pause
        yield return new WaitForSeconds(0.05f);

        // --- LUNGE ---
        Vector3 lungePos = backPos + transform.up * lungeDistance;
        t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / lungeTime;
            transform.position = Vector3.Lerp(backPos, lungePos, t);
            yield return null;
        }

        isAttacking = false;

        time = 0;
    }*/


}
