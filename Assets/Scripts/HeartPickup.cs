using UnityEngine;
using UnityEngine.Events;

public class HeartPickup : MonoBehaviour
{
    [SerializeField] private UnityEvent onCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null || playerController.GetPlayerType() != PlayerType.Bubble) return;

        onCollected?.Invoke();
        GameManager.instance.CollectHeart();
        Destroy(gameObject);
    }
}