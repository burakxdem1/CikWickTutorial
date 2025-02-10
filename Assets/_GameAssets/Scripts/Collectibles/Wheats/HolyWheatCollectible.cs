using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO wheatDesignSo;

    [SerializeField] private PlayerMovement playerMovement;

    public void Collect()
    {
        playerMovement.SetJumpForce(wheatDesignSo.IncreaseDecreaseMultiplier, wheatDesignSo.ResetDuration);
        Destroy(gameObject);
    }
}
