using UnityEngine;

public class BodyWiggle : MonoBehaviour
{
    [Header("Neck Bones (in order Root → End)")]
    public Transform neckRoot;
    public Transform neckMid;
    public Transform neckEnd;

    [Header("Jelly Parameters")]
    public float stiffness = 6f; // how fast it stabilizes
    public float damping = 4f;   // reduces oscillation
    public float maxAngle = 25f; // max bend
    public float followSpeed = 6f;

    [Header("Optional motion injection")]
    public Transform bodyRoot;  // reference to bird body
    public float stepSwayAmount = 0.15f;
    public float stepSwaySpeed = 5f;

    Vector3 targetDirection;
    Vector3 velocityRoot;
    Vector3 velocityMid;
    Vector3 velocityEnd;
    float stepTime;

    void Start()
    {
        if (bodyRoot == null)
            bodyRoot = transform.parent;

        // Initial direction = forward
        targetDirection = bodyRoot.forward;
    }

    void Update()
    {
        // Optional sway if body has any side-to-side oscillation
        ApplyStepSway();

        // Smoothly update target based on body's forward direction
        targetDirection = Vector3.Slerp(
            targetDirection,
            bodyRoot.forward,
            Time.deltaTime * followSpeed
        );

        // Apply spring motion per-bone
        ApplyJelly(neckRoot, ref velocityRoot);
        ApplyJelly(neckMid, ref velocityMid);
        ApplyJelly(neckEnd, ref velocityEnd);
    }

    void ApplyJelly(Transform bone, ref Vector3 boneVelocity)
    {
        // desired orientation
        Quaternion desiredRot = Quaternion.LookRotation(targetDirection, bodyRoot.up);
        Quaternion current = bone.rotation;

        // convert to angle/axis
        Quaternion delta = desiredRot * Quaternion.Inverse(current);
        delta.ToAngleAxis(out float angle, out Vector3 axis);

        // clamp
        if (angle > 180f) angle -= 360f;
        angle = Mathf.Clamp(angle, -maxAngle, maxAngle);

        Vector3 angularDisplacement = axis.normalized * (angle * Mathf.Deg2Rad);

        // spring dynamics
        Vector3 acceleration = angularDisplacement * stiffness - boneVelocity * damping;
        boneVelocity += acceleration * Time.deltaTime;

        // integrate
        bone.rotation = Quaternion.AngleAxis(boneVelocity.magnitude * Mathf.Rad2Deg,
                                             boneVelocity.normalized) * bone.rotation;
    }

    void ApplyStepSway()
    {
        // You can feed in your own stepping logic here later
        // For now, generate small procedural oscillation based on body velocity
        float speed = bodyRoot.GetComponent<Rigidbody>()?.linearVelocity.magnitude ?? 0f;

        stepTime += Time.deltaTime * stepSwaySpeed * Mathf.Lerp(0f, 1f, speed);

        float sway = Mathf.Sin(stepTime) * stepSwayAmount;

        targetDirection += bodyRoot.right * sway;
        targetDirection.Normalize();
    }
}
