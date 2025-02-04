using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState currentPlayerState = PlayerState.Idle;

    private void Start()
    {
        currentPlayerState = PlayerState.Idle;
    }

    public void ChangeState(PlayerState newPlayerState)
    {
        if (currentPlayerState != newPlayerState) 
        {
            currentPlayerState = newPlayerState;
        }
    }

    public PlayerState GetCurrentState()
    {
        return currentPlayerState;
    }
}
