using UnityEngine;
using System.Collections.Generic;

public class CheckpointMover : MonoBehaviour
{
    [Header("Checkpoints")]
    public List<Transform> checkpoints;
    public Transform bonusCheckpoint1;
    public Transform bonusCheckpoint2;

    [Header("Movement Settings")]
    public float speed = 5f;
    public float reachDistance = 0.1f;

    [Header("Custom Inputs")]
    public KeyCode input1 = KeyCode.LeftShift;
    public KeyCode input2 = KeyCode.Space;

    private int currentCheckpointIndex = 0;
    private bool goingToBonus = false;
    private bool goingToReset = false;
    private bool movementStopped = true; // Start stopped by default
    private bool holdingInput1 = false;
    private bool holdingInput2 = false;

    private Transform currentTarget;

    void Update()
    {
        HandleCustomInputs();

        // Prevent any movement unless it's specifically triggered
        if (movementStopped && !goingToBonus && !goingToReset && !holdingInput1 && !holdingInput2)
            return;

        // Input1: Move through checkpoint sequence
        if (holdingInput1)
        {
            if (checkpoints.Count == 0 || currentCheckpointIndex >= checkpoints.Count)
                return;

            currentTarget = checkpoints[currentCheckpointIndex];
            MoveTowards(currentTarget);

            if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
            {
                currentCheckpointIndex++;
            }

            return;
        }

        // Input2: Move toward bonusCheckpoint2
        if (holdingInput2 && bonusCheckpoint2 != null)
        {
            currentTarget = bonusCheckpoint2;
            MoveTowards(currentTarget);
            return;
        }

        // Going to bonus (from B, N, or Input1 release)
        if (goingToBonus && currentTarget != null)
        {
            MoveTowards(currentTarget);

            if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
            {
                SetMovementStopped(true);
                goingToBonus = false;
            }

            return;
        }

        // Returning to first checkpoint (from M)
        if (goingToReset && currentTarget != null)
        {
            MoveTowards(currentTarget);

            if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
            {
                goingToReset = false;
                currentCheckpointIndex = 1;
            }

            return;
        }

        // Default checkpoint sequence (only if resumed from M)
        if (checkpoints.Count == 0 || currentCheckpointIndex >= checkpoints.Count)
            return;

        currentTarget = checkpoints[currentCheckpointIndex];
        MoveTowards(currentTarget);

        if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
        {
            currentCheckpointIndex++;
        }
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            GoToBonusCheckpoint(bonusCheckpoint1);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            GoToBonusCheckpoint(bonusCheckpoint2);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            ResumeFromFirstCheckpoint();
        }
    }

    private void HandleCustomInputs()
    {
        // Input1: hold to follow normal checkpoints from start, release to go to bonus 1
        if (Input.GetKeyDown(input1))
        {
            currentCheckpointIndex = 0;
            SetMovementStopped(false);
            holdingInput1 = true;
        }

        if (Input.GetKeyUp(input1))
        {
            holdingInput1 = false;
            GoToBonusCheckpoint(bonusCheckpoint1);
        }

        // Input2: hold to go to bonus 2, release to stop instantly
        if (Input.GetKeyDown(input2))
        {
            SetMovementStopped(false);
            holdingInput2 = true;
        }

        if (Input.GetKeyUp(input2))
        {
            holdingInput2 = false;
            SetMovementStopped(true);
        }
    }

    private void MoveTowards(Transform target)
    {
        if (target == null) return;

        float currentSpeed = speed;

        // Double speed ONLY when going to bonusCheckpoint1
        if (goingToBonus && target == bonusCheckpoint1)
        {
            currentSpeed *= 5f;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    public void GoToBonusCheckpoint(Transform bonus)
    {
        if (bonus != null)
        {
            currentTarget = bonus;
            goingToBonus = true;
            goingToReset = false;
            SetMovementStopped(false);
        }
    }

    public void ResumeFromFirstCheckpoint()
    {
        if (checkpoints.Count == 0) return;

        currentCheckpointIndex = 0;
        currentTarget = checkpoints[0];
        goingToReset = true;
        goingToBonus = false;
        SetMovementStopped(false);
    }

    public void SetMovementStopped(bool stopped)
    {
        movementStopped = stopped;
    }
}
