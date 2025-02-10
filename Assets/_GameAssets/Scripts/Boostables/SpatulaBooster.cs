using UnityEngine;

public class SpatulaBooster : MonoBehaviour, IBoostables
{
    [Header("References")]
    [SerializeField] private Animator spatulaAnimator;

    [Header("Settings")]
    [SerializeField] private float jumpForce;

    private bool isActivated;

    public void Boost(PlayerMovement playerMovement)
    {
        if(isActivated) { return; }

        BoostAnim();
        Rigidbody rb = playerMovement.ReturnRb();

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.forward * jumpForce, ForceMode.Impulse);

        isActivated = true;
        Invoke(nameof(ResetBoostAnim), 0.2f);
    }

    private void BoostAnim()
    {
        spatulaAnimator.SetTrigger("IsSpatulaJumping");
    }

    private void ResetBoostAnim()
    {
        isActivated = false;
    }
}
