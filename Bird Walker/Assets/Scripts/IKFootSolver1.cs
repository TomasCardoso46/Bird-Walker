using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform body;                 // The main body (spider, creature, etc.)
    [SerializeField] private IKFootSolver otherFoot;         // The opposite foot
    [SerializeField] private LayerMask terrainLayer;         // The ground layer

    [Header("Step Settings")]
    [SerializeField] private float speed = 1f;               // Step speed
    [SerializeField] private float stepDistance = 1f;        // Distance before taking a new step
    [SerializeField] private float stepLength = 0.5f;        // Forward distance per step
    [SerializeField] private float stepHeight = 0.2f;        // Height of the arc during a step
    [SerializeField] private Vector3 footOffset = new Vector3(0, 0.05f, 0);  // Slight lift

    private float footSpacing;
    private Vector3 oldPosition, currentPosition, newPosition;
    private Vector3 oldNormal, currentNormal, newNormal;
    private float lerp;

    private void Start()
    {
        // Get the local X offset of the foot relative to the body (side-to-side spacing)
        if (body != null)
            footSpacing = body.InverseTransformPoint(transform.position).x;
        else
            footSpacing = transform.localPosition.x;

        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1f;
    }

    private void Update()
    {
        transform.position = currentPosition;
        transform.up = currentNormal;

        // --- Generate a proper ray origin ---
        Vector3 rayOrigin = body.position + (body.right * footSpacing) + Vector3.up * 1f; // Raise a bit above ground
        Ray ray = new Ray(rayOrigin, Vector3.down);

        // --- Raycast to find ground ---
        if (Physics.Raycast(ray, out RaycastHit info, 5f, terrainLayer.value))
        {
            // Debug visualization
            Debug.DrawRay(ray.origin, ray.direction * info.distance, Color.green);
            Debug.DrawLine(ray.origin, info.point, Color.yellow);

            float distanceFromTarget = Vector3.Distance(newPosition, info.point);

            // Check if it's time to step
            if (distanceFromTarget > stepDistance && !otherFoot.IsMoving() && lerp >= 1f)
            {
                lerp = 0f;

                // Determine step direction based on body forward
                int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                newPosition = info.point + (body.forward * stepLength * direction) + footOffset;
                newNormal = info.normal;
            }
        }
        else
        {
            // If no hit, visualize in red
            Debug.DrawRay(ray.origin, ray.direction * 5f, Color.red);
        }

        // --- Smooth stepping ---
        if (lerp < 1f)
        {
            Vector3 tempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            tempPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = tempPosition;
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
            oldNormal = newNormal;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.05f);
    }

    public bool IsMoving()
    {
        return lerp < 1f;
    }
}
