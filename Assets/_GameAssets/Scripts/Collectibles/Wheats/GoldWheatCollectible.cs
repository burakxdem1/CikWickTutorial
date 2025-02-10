using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesignSO wheatDesignSo;

    [SerializeField] private PlayerMovement playerMovement;

    public void Collect()
    {
        playerMovement.SetMovementSpeed(wheatDesignSo.IncreaseDecreaseMultiplier, wheatDesignSo.ResetDuration);
        Destroy(gameObject);
    }
}
