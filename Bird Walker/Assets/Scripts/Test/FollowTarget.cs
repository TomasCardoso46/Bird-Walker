using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float followSpeed = 10f;
    public float rotateSpeed = 10f;

    [Header("Offsets")]
    public Vector3 positionOffset;         // local position offset relative to the target
    public Vector3 rotationOffsetEuler;    // rotation offset in euler angles

    private Quaternion rotationOffset;

    void Start()
    {
        // Pre-calc the rotation offset as a quaternion
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }

    void Update()
    {
        if (target == null)
            return;

        // --- Position follow ---
        Vector3 desiredPosition = target.position + target.TransformDirection(positionOffset);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        // --- Rotation follow ---
        Quaternion desiredRotation = target.rotation * rotationOffset;
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotateSpeed * Time.deltaTime);
    }

    // Update rotationOffset if inspector changes rotationOffsetEuler during play
    void OnValidate()
    {
        rotationOffset = Quaternion.Euler(rotationOffsetEuler);
    }
}
