using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] float moveSpeed = 6f;

    [SerializeField] Transform groundCheck;

    [SerializeField] LayerMask ground;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.linearVelocity = new Vector3(horizontalInput * moveSpeed, rb.linearVelocity.y, verticalInput * moveSpeed);

        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, moveSpeed, rb.linearVelocity.z);
        }

        if (Input.GetKey("up")) {
            rb.linearVelocity = new Vector3(0, 0, 5f);
        }

        if (Input.GetKey("down")) {
            rb.linearVelocity = new Vector3(0, 0, -5f);
        }

        if (Input.GetKey("right")) {
            rb.linearVelocity = new Vector3(5f, 0, 0);
        }

        if (Input.GetKey("left")) {
            rb.linearVelocity = new Vector3(-5f, 0, 0);
        }
    }

    bool IsGrounded() {
        return !Physics.CheckSphere(groundCheck.position, .1f, ground);
    }
}
