using UnityEngine;

public class Anzol : MonoBehaviour
{
    [Header("Targets (must have colliders marked as Triggers)")]
    public Transform Ptarget;
    public Transform Qtarget;

    [Header("Movement Settings")]
    public float moveSpeed = 200f;

    private Transform currentTarget;

    private void Update()
    {
        if (Input.GetKey(KeyCode.P) && Ptarget != null)
        {
            currentTarget = Ptarget;
        }
        else if (Input.GetKey(KeyCode.Q) && Qtarget != null)
        {
            currentTarget = Qtarget;
        }
        else
        {
            currentTarget = null;
        }

        if (currentTarget != null)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == currentTarget)
        {
            currentTarget = null; // stop moving once touched
        }
    }
}
