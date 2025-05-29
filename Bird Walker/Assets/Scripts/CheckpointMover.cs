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

    private int currentCheckpointIndex = 0;
    private bool goingToBonus = false;
    private bool movementStopped = false;
    private Transform currentTarget;

    void Update()
    {
        if (movementStopped && !goingToBonus) return;

        if (goingToBonus && currentTarget != null)
        {
            MoveTowards(currentTarget);

            if (Vector3.Distance(transform.position, currentTarget.position) < reachDistance)
            {
                SetMovementStopped(true); // Stop after reaching bonus
                goingToBonus = false; // Clear bonus flag after reaching it
            }

            return;
        }

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
            ResetToFirstCheckpoint();
        }
    }

    private void MoveTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    public void GoToBonusCheckpoint(Transform bonus)
    {
        if (bonus != null)
        {
            currentTarget = bonus;
            goingToBonus = true;
            SetMovementStopped(false); // Allow movement to bonus even if previously stopped
        }
    }

    public void ResetToFirstCheckpoint()
    {
        if (checkpoints.Count == 0) return;

        transform.position = checkpoints[0].position;
        currentCheckpointIndex = 0;
        currentTarget = checkpoints[0];
        goingToBonus = false;
        SetMovementStopped(false);
    }

    public void SetMovementStopped(bool stopped)
    {
        movementStopped = stopped;
    }
}
