using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovementController : MonoBehaviour
{
    public bool isMoving;
    public bool isIdling;
    private Vector2 moveTarget;
    public float moveSpeed = 0.4f;

    private Vector2 originPoint;
    public float maxXTravelDistanceFromOrigin = 5f;
    public float maxYTravelDistanceFromOrigin = 0.2f;
    private float moveStoppingDistance = 0.1f;

    public float minIdleTime = 1f;
    public float maxIdleTime = 5f;
    private float remainingIdleTime = 0f;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        isMoving = false;
        moveTarget = transform.position;
        originPoint = transform.position;

        remainingIdleTime = Random.Range(minIdleTime, maxIdleTime);

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isMoving)
            MoveTowardsCurrentTarget();
        else
            if (remainingIdleTime <= 0f)
            AssignNewMovementTarget();

        remainingIdleTime -= Time.deltaTime;

        animator.SetBool("isWalking", isMoving);
    }

    public void MoveTowardsCurrentTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, moveTarget, moveSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, moveTarget);
        if (distance < moveStoppingDistance)
        {
            isMoving = false;
            remainingIdleTime = Random.Range(minIdleTime, maxIdleTime);
        }
    }

    public void AssignNewMovementTarget()
    {
        moveTarget = originPoint + new Vector2(
            x: Random.Range(-maxXTravelDistanceFromOrigin, maxXTravelDistanceFromOrigin),
            y: Random.Range(-maxYTravelDistanceFromOrigin, maxYTravelDistanceFromOrigin)
        );
        isMoving = true;

        bool moveTargetIsLeft = moveTarget.x < transform.position.x;
        spriteRenderer.flipX = moveTargetIsLeft;
    }
}
