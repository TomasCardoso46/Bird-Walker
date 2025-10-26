using UnityEngine;

public class ToggleComponents : MonoBehaviour
{
    [Header("References")]
    public Animator targetAnimator;
    public Rigidbody targetRigidbody;

    private bool isToggled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleState();
        }
    }

    public void ToggleState()
    {
        isToggled = !isToggled;

        // Disable or enable the animator
        if (targetAnimator != null)
            targetAnimator.enabled = !isToggled;

        if (targetRigidbody != null)
        {
            // Turn gravity on when toggled, off otherwise
            targetRigidbody.useGravity = isToggled;

            // Disable all constraints except FreezeRotationY and FreezeRotationZ when toggled
            if (isToggled)
            {
                targetRigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            }
            else
            {
                // No constraints when untoggled
                targetRigidbody.constraints = RigidbodyConstraints.None;
            }
        }

        Debug.Log("Toggled state: " + isToggled);
    }
}
