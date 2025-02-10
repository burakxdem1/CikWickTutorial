using UnityEngine;

[CreateAssetMenu(fileName = "WheatDesignSO", menuName = "ScriptibleObjects/WheatDesignSo")]
public class WheatDesignSO : ScriptableObject
{
    [SerializeField] private float increaseDecreaseMultiplier;
    [SerializeField] private float resetDuration;

    public float IncreaseDecreaseMultiplier => increaseDecreaseMultiplier;
    public float ResetDuration => resetDuration;
}
