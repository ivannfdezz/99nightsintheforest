using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerJumpCollision : MonoBehaviour
{
    public float jumpForce = 5f;          // jump strength
    private Rigidbody rb;
    private bool isGrounded = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // keep upright
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        Vector3 vel = rb.linearVelocity;
        vel.y = 0f; // reset vertical velocity for consistent jumps
        rb.linearVelocity = vel;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false; // prevent double jumps
    }

    // Called while colliding with something
    private void OnCollisionStay(Collision collision)
    {
        // Check if collision is mostly from below
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                isGrounded = true;
                break;
            }
        }
    }

    // Optional: reset grounded on exit
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
