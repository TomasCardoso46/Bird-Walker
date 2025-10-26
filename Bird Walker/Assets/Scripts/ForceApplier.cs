using UnityEngine;

public class ForceApplier : MonoBehaviour
{
    [Header("References")]
    public ToggleComponents toggleScript;  // Reference to other script

    [Header("Force Settings")]
    public ForceMode forceMode = ForceMode.Impulse;

    private Rigidbody targetRigidbody;

    void Awake()
    {
        // Automatically get the Rigidbody on this object
        targetRigidbody = GetComponent<Rigidbody>();

        if (targetRigidbody == null)
            Debug.LogWarning("No Rigidbody found on " + gameObject.name + "! ForceApplier needs one.");
    }

    /// <summary>
    /// Applies a force to this object's Rigidbody in the given direction.
    /// </summary>
    public void ApplyForce(float ForceX, float ForceY, float ForceZ)
    {
        // 1️⃣ Call the ToggleState method from the other script first
        if (toggleScript != null)
        {
            toggleScript.ToggleState();
        }
        else
        {
            Debug.LogWarning("No ToggleComponents script assigned to ForceApplier on " + gameObject.name);
        }

        // 2️⃣ Apply the given force
        if (targetRigidbody == null)
            return;

        Vector3 finalForce = new Vector3(ForceX, ForceY, ForceZ);
        targetRigidbody.AddForce(finalForce, forceMode);

        Debug.Log($"Applied force {finalForce} to {gameObject.name}");
    }
}
