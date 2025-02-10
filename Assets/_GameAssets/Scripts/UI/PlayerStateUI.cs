using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private RectTransform playerWalkingTransform;
    [SerializeField] private RectTransform playerSlidingTransform;

    [Header("Sprites")]
    [SerializeField] private Sprite PlayerWalkingActiveSprite;
    [SerializeField] private Sprite PlayerWalkingPassiveSprite;
    [SerializeField] private Sprite PlayerSlidingActiveSprite;
    [SerializeField] private Sprite PlayersSlidingPassiveSprite;

    [Header("Settings")]
    [SerializeField] private float moveDur;
    [SerializeField] private Ease moveEase;

    private Image playerWalkingImage;
    private Image playerSlidingImage;

    private void Awake()
    {
        playerWalkingImage = playerWalkingTransform.GetComponent<Image>();
        playerSlidingImage = playerSlidingTransform.GetComponent<Image>();
    }

    private void Start()
    {
        playerMovement.OnPlayerStateChanged += GetOnPlayerChangedState;

        SetStatesUserInterfaces(PlayerWalkingActiveSprite, PlayersSlidingPassiveSprite, playerWalkingTransform, playerSlidingTransform);
    }

    private void GetOnPlayerChangedState(PlayerState playerState)
    {
        switch (playerState) 
        {
            case PlayerState.Idle:
            case PlayerState.Move:
                SetStatesUserInterfaces(PlayerWalkingActiveSprite, PlayersSlidingPassiveSprite, playerWalkingTransform, playerSlidingTransform);
                break;

            case PlayerState.SlideIdle:
            case PlayerState.Slide:
                SetStatesUserInterfaces(PlayerWalkingPassiveSprite, PlayerSlidingActiveSprite, playerSlidingTransform, playerWalkingTransform);
                break;
        }
    }

    private void SetStatesUserInterfaces(Sprite playerWalkingSprite, Sprite playerSlidingSprite, RectTransform activeTransform, RectTransform passiveTransform)
    {
        playerWalkingImage.sprite = playerWalkingSprite;
        playerSlidingImage.sprite = playerSlidingSprite;

        activeTransform.DOAnchorPosX(-25f, moveDur).SetEase(moveEase);
        passiveTransform.DOAnchorPosX(-90f, moveDur).SetEase(moveEase);
    }
}
