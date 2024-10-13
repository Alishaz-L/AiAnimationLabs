using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField]
    public PathManager pathManager;

    List<waypoint> thePath;
    waypoint target;

    public float MoveSpeed;
    public float RotateSpeed;

    public Animator animator;
    bool isSprinting;

    // Initialize sprinting state and set the first waypoint target
    void Start()
    {
        isSprinting = false;
        animator.SetBool("isSprinting", isSprinting);

        thePath = pathManager.GetPath();
        if (thePath != null && thePath.Count > 0)
        {
            // set starting target to the first waypoint
            target = thePath[0];
        }
    }

    // Rotate towards the current target based on RotateSpeed
    void rotateTowardsTarget()
    {
        float stepSize = RotateSpeed * Time.deltaTime;

        Vector3 targetDir = target.pos - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, stepSize, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Move forward towards the current target based on MoveSpeed
    void moveForward()
    {
        float stepSize = Time.deltaTime * MoveSpeed;
        float distanceToTarget = Vector3.Distance(transform.position, target.pos);
        if (distanceToTarget < stepSize)
        {
            // we will overshoot the target,
            // so we should do something smarter here
            return;
        }
        // take a step forward
        Vector3 moveDir = Vector3.forward;
        transform.Translate(moveDir * stepSize);
    }

    // Update sprinting state and movement if sprinting is active
    void Update()
    {
        if (Input.anyKeyDown)
        {
            isSprinting = !isSprinting;
            animator.SetBool("isSprinting", isSprinting);
        }
        if (isSprinting)
        {
            rotateTowardsTarget();
            moveForward();
        }
    }

    // Change the target to the next waypoint when triggered
    private void OnTriggerEnter(Collider other)
    {
        target = pathManager.GetNextTarget();
    }
}
