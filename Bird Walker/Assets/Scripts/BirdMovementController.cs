using UnityEngine;

public class BirdMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Transform body;
    [SerializeField] private float moveSpeed = 2f;

    [Header("Axis Restriction")]
    [Tooltip("1 means allowed, 0 means locked. Example: (0,0,1) = forward/back only")]
    [SerializeField] private Vector3 allowedAxis = new Vector3(0, 0, 1);

    [Header("Input Settings")]
    [SerializeField] private KeyCode leftLegKey = KeyCode.Q;
    [SerializeField] private KeyCode rightLegKey = KeyCode.P;

    private bool leftStep, rightStep;

    void Update()
    {
        Vector3 moveInput = Vector3.zero;

        // Separate leg inputs
        if (Input.GetKeyDown(leftLegKey))
            leftStep = true;
        if (Input.GetKeyDown(rightLegKey))
            rightStep = true;

        // Only move one leg at a time
        if (leftStep ^ rightStep)
        {
            moveInput += Vector3.forward;
        }

        // Cancel movement if both are pressed
        if (Input.GetKey(leftLegKey) && Input.GetKey(rightLegKey))
        {
            moveInput = Vector3.zero;
        }

        // Restrict motion to the chosen axis
        Vector3 finalMovement = Vector3.Scale(moveInput, allowedAxis.normalized);
        body.position += body.TransformDirection(finalMovement) * moveSpeed * Time.deltaTime;

        // Reset step triggers
        leftStep = rightStep = false;
    }
}
