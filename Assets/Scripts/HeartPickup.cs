using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(FloatingMotion))]
public class HeartPickup : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollected;
    [SerializeField] private PulseEffect pulseEffect;

    private void Start()
    {
        if (pulseEffect != null)
        {
            pulseEffect.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null || playerController.GetPlayerType() != PlayerType.Bubble) return;

        onCollected?.Invoke();
        GameManager.instance.CollectHeart();
        Destroy(gameObject);
    }
}