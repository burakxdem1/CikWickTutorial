using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private Transform orientation;

    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed;

    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCd;
    private bool canJump = true;

    [Header("Ground Check Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody rb;

    private Vector3 moveDir;

    private float horizontalInput;
    private float verticalInput;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        SetInputs();
    }

    private void FixedUpdate()
    {
        SetMovement();
    }

    private void SetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && canJump && IsGrounded()) 
        {

            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCd);
        }
    }

    private void SetMovement()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDir.normalized * moveSpeed, ForceMode.Force );
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }
}
