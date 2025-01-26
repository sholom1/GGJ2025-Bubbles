using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BubbleController : MonoBehaviour
{
    [Header("Visual Feedback")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color readyToPopColor = new Color(1f, 0.5f, 0.5f, 1f); // Light pink
    [SerializeField] private GameObject heartIndicator;
    
    private SpriteRenderer spriteRenderer;
    private string urchinTag = "Player";

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (heartIndicator != null)
        {
            heartIndicator.SetActive(false);
        }
    }

    private void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.onAllHeartsCollected.AddListener(OnReadyToPop);
        }
    }

    private void OnReadyToPop()
    {
        spriteRenderer.color = readyToPopColor;
        if (heartIndicator != null)
        {
            heartIndicator.SetActive(true);
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