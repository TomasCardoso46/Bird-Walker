using UnityEngine;

public class BodySpring : MonoBehaviour
{
    public Transform[] neckBones;
    public float stiffness = 10f;
    public float damping = 3f;
    public float wiggleAmount = 20f;

    private Quaternion[] initialRot;
    private Vector3[] angularVelocity; // angular velocity in degrees per second

    void Start()
    {
        initialRot = new Quaternion[neckBones.Length];
        angularVelocity = new Vector3[neckBones.Length];

        for (int i = 0; i < neckBones.Length; i++)
        {
            initialRot[i] = neckBones[i].localRotation;
            angularVelocity[i] = Vector3.zero;
        }
    }

    void Update()
    {
        // Add impulse when Q or P are released
        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.P))
        {
            for (int i = 0; i < neckBones.Length; i++)
            {
                angularVelocity[i] += Random.insideUnitSphere * wiggleAmount;
            }
        }

        // Spring solve
        for (int i = 0; i < neckBones.Length; i++)
        {
            Quaternion delta = initialRot[i] * Quaternion.Inverse(neckBones[i].localRotation);
            delta.ToAngleAxis(out float angle, out Vector3 axis);

            if (angle > 180f) angle -= 360f;

            Vector3 springForce = axis * angle * stiffness;
            Vector3 dampingForce = angularVelocity[i] * damping;

            angularVelocity[i] += (springForce - dampingForce) * Time.deltaTime;

            // Stop jittering when small
            if (angularVelocity[i].magnitude < 0.01f && Mathf.Abs(angle) < 0.5f)
            {
                angularVelocity[i] = Vector3.zero;
                neckBones[i].localRotation = initialRot[i];
                continue;
            }

            neckBones[i].localRotation =
                Quaternion.AngleAxis(angularVelocity[i].magnitude * Time.deltaTime, angularVelocity[i].normalized)
                * neckBones[i].localRotation;
        }
    }
}
