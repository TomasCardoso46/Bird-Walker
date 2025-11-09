using UnityEngine;

/// <summary>
/// Physics-based neck/head wobble with pitch and roll only.
/// Twist around forward axis is locked.
/// </summary>
public class JellyHeadPitchRollFixed : MonoBehaviour
{
    [Header("Head/Neck Bones")]
    public Transform neckRoot;
    public Transform neckMid;
    public Transform neckEnd;

    [Header("Body Reference")]
    public Transform bodyRoot;

    [Header("Physics Settings")]
    public float stiffness = 6f;
    public float damping = 4f;
    public float maxPitch = 25f; // up/down
    public float maxRoll = 15f;  // left/right

    [Header("Debug Jiggle")]
    public float debugJigglePitch = 15f;
    public float debugJiggleRoll = 10f;

    private Vector3 velocityRoot;
    private Vector3 velocityMid;
    private Vector3 velocityEnd;
    private bool debugJiggle = false;
    private float jiggleTime = 0f;

    private Quaternion initialRoot;
    private Quaternion initialMid;
    private Quaternion initialEnd;

    void Start()
    {
        if (bodyRoot == null) bodyRoot = transform.parent;

        if (neckRoot != null) initialRoot = neckRoot.localRotation;
        if (neckMid != null) initialMid = neckMid.localRotation;
        if (neckEnd != null) initialEnd = neckEnd.localRotation;
    }

    void Update()
    {
        // Trigger debug jiggle
        if (Input.GetKeyDown(KeyCode.J))
        {
            debugJiggle = true;
            jiggleTime = 0f;
        }

        Vector3 targetPitchRoll = Vector3.zero;

        if (debugJiggle)
        {
            jiggleTime += Time.deltaTime;
            targetPitchRoll.x = Mathf.Sin(jiggleTime * 2f) * debugJigglePitch;
            targetPitchRoll.z = Mathf.Sin(jiggleTime * 3f) * debugJiggleRoll;

            if (jiggleTime > Mathf.PI * 2f) debugJiggle = false;
        }

        ApplyJellyXZ(neckRoot, ref velocityRoot, initialRoot, targetPitchRoll);
        ApplyJellyXZ(neckMid, ref velocityMid, initialMid, targetPitchRoll * 0.7f);
        ApplyJellyXZ(neckEnd, ref velocityEnd, initialEnd, targetPitchRoll * 0.5f);
    }

    void ApplyJellyXZ(Transform bone, ref Vector3 velocity, Quaternion initialRotation, Vector3 targetEuler)
    {
        if (bone == null) return;

        // Current local rotation
        Quaternion currentRot = bone.localRotation;

        // Target rotation relative to initial
        Quaternion targetRot = initialRotation * Quaternion.Euler(
            Mathf.Clamp(targetEuler.x, -maxPitch, maxPitch),
            0f, // no yaw
            Mathf.Clamp(targetEuler.z, -maxRoll, maxRoll)
        );

        // Compute delta rotation
        Quaternion delta = targetRot * Quaternion.Inverse(currentRot);

        // Convert to axis-angle
        delta.ToAngleAxis(out float angle, out Vector3 axis);
        if (angle > 180f) angle -= 360f;
        axis.Normalize();

        // Lock twist around forward
        axis = bone.InverseTransformDirection(axis);
        axis.y = 0f; // lock vertical rotation (twist)
        axis = bone.TransformDirection(axis);

        // Spring-damper physics
        Vector3 angularDisplacement = axis * Mathf.Deg2Rad * angle;
        Vector3 acceleration = angularDisplacement * stiffness - velocity * damping;
        velocity += acceleration * Time.deltaTime;

        bone.localRotation = currentRot * Quaternion.Euler(velocity * Time.deltaTime * Mathf.Rad2Deg);
    }
}
