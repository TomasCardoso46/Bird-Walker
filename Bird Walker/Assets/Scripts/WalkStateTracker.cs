using UnityEngine;

/// <summary>
/// Tracks whether the bird is walking based solely on CheckpointMover input.
/// </summary>
public class WalkStateTracker : MonoBehaviour
{
    [Header("CheckpointMover References")]
    public CheckpointMover leftLegMover;
    public CheckpointMover rightLegMover;

    /// <summary>
    /// True when at least one leg is moving according to player input.
    /// </summary>
    public bool IsWalking { get; private set; }

    void Update()
    {
        bool leftWalking = leftLegMover != null && (leftLegMover.holdingInput1 || leftLegMover.holdingInput2);
        bool rightWalking = rightLegMover != null && (rightLegMover.holdingInput1 || rightLegMover.holdingInput2);

        IsWalking = leftWalking || rightWalking;
    }
}
