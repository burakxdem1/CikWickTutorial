using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public event Action OnPlayerJump;
    public event Action OnPlayerFall;
    public event Action<PlayerState> OnPlayerStateChanged;
    
    [Header("References")]
    [SerializeField]private Transform orientation;
    private Rigidbody rb;
    private StateController stateController;

    [Header("Keybinds")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;


    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed;
    [SerializeField] private KeyCode moveKey = KeyCode.E;


    [Header("Slide Settings")]
    [SerializeField] private KeyCode slideKey = KeyCode.Q;
    [SerializeField] private float slideMultiplier;
    [SerializeField] private float slideDrag;
    private bool isSliding;


    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airMultiplier;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpCd;
    private bool canJump = true;
    private bool isFalling;

    [Header("Ground Check Settings")]
    [SerializeField] private float groundDrag;
    [SerializeField] private bool isGrounded;
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask whatIsGround;

    private float startMoveSpeed;
    private float startJumpForce;

    private Vector3 moveDir;

    private float horizontalInput;
    private float verticalInput;


    private void Awake()
    {
        stateController = GetComponent<StateController>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;


        startMoveSpeed = moveSpeed; 
        startJumpForce = jumpForce;
    }

    private void Update()
    {
        isGrounded = IsGrounded();
        isFalling = IsFalling();
        FallingBool();
        SetInputs();
        SetState();
        SetDrag();
        LimitSpeed();
    }

    private void FixedUpdate()
    {
        SetMovement();
    }

    private void SetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey))
        {
            isSliding = true;
        } 
        else if (Input.GetKeyDown(moveKey)) 
        {
            isSliding = false;
        }
        else if (Input.GetKey(jumpKey) && canJump && IsGrounded()) 
        {

            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCd);
        }
    }

    private void SetState()
    {
        var moveDir = GetMovementdirection();
        var isGrounded = IsGrounded();
        var isSliding = IsSliding();
        var currentState = stateController.GetCurrentState();

        var newState = currentState switch 
        {
            _ when moveDir == Vector3.zero && isGrounded && !isSliding => PlayerState.Idle,
            _ when moveDir != Vector3.zero && isGrounded && !isSliding => PlayerState.Move,
            _ when moveDir != Vector3.zero && isGrounded && isSliding => PlayerState.Slide,
            _ when moveDir == Vector3.zero && isSliding && isSliding => PlayerState.SlideIdle,
            _ when !canJump && !isGrounded => PlayerState.Jump,
            _ when isFalling => PlayerState.Fall,
            _ => currentState
        };

        if (newState != currentState) 
        {
            stateController.ChangeState(newState);
            OnPlayerStateChanged?.Invoke(newState);
        }

        Debug.Log(currentState);
    }

    private void SetMovement()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        float forceMultiplier = stateController.GetCurrentState() switch 
        {
            PlayerState.Idle => 1f,
            PlayerState.Slide => slideMultiplier,
            PlayerState.Jump => airMultiplier,
            _ => 1f
        };

        rb.AddForce(moveDir.normalized * moveSpeed * forceMultiplier, ForceMode.Force);


    }

    private void SetDrag()
    {
        rb.linearDamping = stateController.GetCurrentState() switch 
        {
            PlayerState.Move => groundDrag,
            PlayerState.Slide => slideDrag,
            PlayerState.Jump => airDrag,
            _ => rb.linearDamping
        };
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(flatVel.magnitude > moveSpeed) 
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        OnPlayerJump?.Invoke();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    #region Help Functions

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private bool IsFalling()
    {
        return !Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void FallingBool()
    {
        if (isFalling) 
        {
            OnPlayerFall.Invoke();
        }
    }

    private Vector3 GetMovementdirection()
    {
        return moveDir.normalized;
    }

    public Rigidbody ReturnRb()
    {
        return rb;
    }

    private bool IsSliding()
    {
        return isSliding;
    }

    public void SetMovementSpeed(float speed, float duration)
    {
        moveSpeed += speed;
        Invoke(nameof(ResetMoveSpeed), duration);
    }

    private void ResetMoveSpeed()
    {
        moveSpeed = startMoveSpeed;
    }

    public void SetJumpForce(float force, float duration)
    {
        jumpForce += force;
        Invoke(nameof(ResetJumpForce), duration);
    }

    private void ResetJumpForce()
    {
        jumpForce = startJumpForce;
    }
    #endregion
}
