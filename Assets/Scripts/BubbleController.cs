using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BubbleController : MonoBehaviour
{
    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color readyToPopColor = new Color(1f, 0.5f, 0.5f, 1f);
    [SerializeField] private GameObject heartIndicator;
    [SerializeField] private VictoryArrow victoryArrow;
    [SerializeField] private PulseEffect pulseEffect;
    
    private SpriteRenderer spriteRenderer;
    private string urchinTag = "Player";
    private PlayerController urchinController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (heartIndicator != null)
        {
            heartIndicator.SetActive(false);
        }
        if (pulseEffect != null)
        {
            pulseEffect.enabled = false;
        }
        if (victoryArrow != null)
        {
            victoryArrow.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onAllHeartsCollected.AddListener(OnReadyToPop);
            
            // Find the urchin player
            var players = FindObjectsOfType<PlayerController>();
            foreach (var player in players)
            {
                if (player.GetPlayerType() == PlayerType.Urchin)
                {
                    urchinController = player;
                    if (pulseEffect != null)
                    {
                        pulseEffect.SetTarget(player.transform);
                    }
                    break;
                }
            }
        }
    }

    private void OnReadyToPop()
    {
        spriteRenderer.color = readyToPopColor;
        
        if (heartIndicator != null)
        {
            heartIndicator.SetActive(true);
        }

        if (pulseEffect != null)
        {
            pulseEffect.enabled = true;
        }

        if (victoryArrow != null && urchinController != null)
        {
            victoryArrow.gameObject.SetActive(true);
            victoryArrow.StartPointingAt(urchinController.transform);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(urchinTag)) return;

        PlayerController otherPlayer = collision.gameObject.GetComponent<PlayerController>();
        if (otherPlayer != null && otherPlayer.GetPlayerType() == PlayerType.Urchin && GameManager.instance != null)
        {
            if (GameManager.instance.CanBubblePop)
            {
                GameManager.instance.BubbleReachedUrchin();
            }
        }
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onAllHeartsCollected.RemoveListener(OnReadyToPop);
        }
    }
}