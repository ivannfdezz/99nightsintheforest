using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class FPSCameraRelativeMover : MonoBehaviour
{
    [Tooltip("Assign the actual Camera (the GameObject with the Camera component). If empty the script will try Camera.main.")]
    public Transform cameraTransform;

    public float moveSpeed;

    private Rigidbody rb;
    private Vector3 inputDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;               // usually wanted for FPS
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Start()
    {
        // If camera not assigned, try Camera.main
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = 8f;
        }
        else
        {
            moveSpeed = 5f;
        }
            // read individual keys
            float x = 0f, z = 0f;
        if (Input.GetKey(KeyCode.A)) x -= 1f;
        if (Input.GetKey(KeyCode.D)) x += 1f;
        if (Input.GetKey(KeyCode.W)) z += 1f;
        if (Input.GetKey(KeyCode.S)) z -= 1f;

        Vector3 rawInput = new Vector3(x, 0f, z);
        if (rawInput.sqrMagnitude > 1f) rawInput.Normalize();

        if (cameraTransform != null)
        {
            // Use camera yaw only — ignore camera pitch
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight   = cameraTransform.right;

            camForward.y = 0f;
            camRight.y   = 0f;
            camForward.Normalize();
            camRight.Normalize();

            inputDir = camForward * rawInput.z + camRight * rawInput.x;

            if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();
        }
        else
        {
            // fallback to local forward/right (player orientation)
            Vector3 f = transform.forward; f.y = 0f; f.Normalize();
            Vector3 r = transform.right;   r.y = 0f; r.Normalize();
            inputDir = f * rawInput.z + r * rawInput.x;
            if (inputDir.sqrMagnitude > 1f) inputDir.Normalize();
        }
    }

    void FixedUpdate()
    {
        Vector3 desiredVel = inputDir * moveSpeed;
        // preserve vertical velocity (gravity/jump)
        rb.linearVelocity = new Vector3(desiredVel.x, rb.linearVelocity.y, desiredVel.z);
    }
}
