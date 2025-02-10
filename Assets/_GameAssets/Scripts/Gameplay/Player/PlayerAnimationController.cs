using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private bool isGrounded;

    private StateController stateController;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        stateController = GetComponent<StateController>();
    }

    private void Start()
    {
        playerMovement.OnPlayerJump += OnPlayerJump;
        playerMovement.OnPlayerFall += OnPlayerFall;
    }


    private void Update()
    {
        SetPlayerAnimations();
        playerMovement.IsGrounded();
        OnPlayerFall();
    }
    private void OnPlayerFall()
    {
        animator.SetBool("IsFalling", true);
        if(playerMovement.IsGrounded()) 
        {
            animator.SetBool("IsFalling", false);
        }
    }


    private void OnPlayerJump()
    {
        animator.SetBool("IsJumping", true);
        Invoke(nameof(ResetJumpAnim), 0.7f);
    }

    private void ResetJumpAnim()
    {
        animator.SetBool("IsJumping", false);
    }

    private void SetPlayerAnimations()
    {
        var currentState = stateController.GetCurrentState();

        switch (currentState) 
        {
            case PlayerState.Idle:
                animator.SetBool("IsSliding", false);
                animator.SetBool("IsMoving", false);
                break;

            case PlayerState.Move:
                animator.SetBool("IsSliding", false);
                animator.SetBool("IsMoving", true);
                break;

            case PlayerState.SlideIdle:
                animator.SetBool("IsSliding", true);
                animator.SetBool("IsSlidingActive", false);
                break;

            case PlayerState.Slide:
                animator.SetBool("IsSliding", true);
                animator.SetBool("IsSlidingActive", true);
                break;
        }
    }


}
