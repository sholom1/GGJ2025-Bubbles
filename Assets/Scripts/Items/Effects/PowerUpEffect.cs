using UnityEngine;
using System.Collections;

public class PowerUpEffect : MonoBehaviour, IItemEffect
{
    [Header("Effect Settings")]
    [SerializeField] private float bubbleSpeedBoostDuration = 3f;
    [SerializeField] private float bubbleSpeedBoostMultiplier = 1.5f;
    [SerializeField] private float urchinRippleRadius = 3f;
    [SerializeField] private float urchinRipplePushForce = 10f;
    [SerializeField] private int rippleCount = 3;
    [SerializeField] private float rippleInterval = 0.2f;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject rippleEffectPrefab;

    private Coroutine activeEffect;

    public void ApplyEffect(GameObject collector, GameObject target)
    {
        var playerController = collector.GetComponent<PlayerController>();
        if (playerController == null) return;

        if (activeEffect != null)
        {
            StopCoroutine(activeEffect);
        }

        if (IsPlayerType(playerController, PlayerType.Bubble))
        {
            activeEffect = StartCoroutine(ApplyBubbleSpeedBoost(playerController));
        }
        else if (IsPlayerType(playerController, PlayerType.Urchin))
        {
            activeEffect = StartCoroutine(CreateRippleWaves(playerController));
        }
    }

    private bool IsPlayerType(PlayerController controller, PlayerType type)
    {
        // Use reflection to get the private _type field
        var field = typeof(PlayerController).GetField("_type", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            return (PlayerType)field.GetValue(controller) == type;
        }
        return false;
    }

    private IEnumerator ApplyBubbleSpeedBoost(PlayerController playerController)
    {
        var attributeController = playerController.GetComponent<PlayerAttributeController>();
        if (attributeController != null)
        {
            // Apply speed boost
            float originalSpeed = attributeController.MoveSpeed;
            attributeController.SetMovementSpeed(bubbleSpeedBoostMultiplier);

            // Wait for duration
            yield return new WaitForSeconds(bubbleSpeedBoostDuration);

            // Reset speed by dividing by the multiplier
            attributeController.SetMovementSpeed(1f / bubbleSpeedBoostMultiplier);
        }
    }

    private IEnumerator CreateRippleWaves(PlayerController playerController)
    {
        for (int i = 0; i < rippleCount; i++)
        {
            CreateRippleWave(playerController);
            yield return new WaitForSeconds(rippleInterval);
        }
    }

    private void CreateRippleWave(PlayerController playerController)
    {
        // Instantiate visual effect if prefab is assigned
        if (rippleEffectPrefab != null)
        {
            Instantiate(rippleEffectPrefab, transform.position, Quaternion.identity);
        }

        // Find and push bubble if in range
        var bubble = GameManager.instance.GetOtherPlayer(playerController);
        if (bubble != null && IsPlayerType(bubble, PlayerType.Bubble))
        {
            float distance = Vector2.Distance(transform.position, bubble.transform.position);
            if (distance <= urchinRippleRadius)
            {
                Vector2 pushDirection = (bubble.transform.position - transform.position).normalized;
                var rb = bubble.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(pushDirection * urchinRipplePushForce, ForceMode2D.Impulse);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw ripple radius in editor for visualization
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, urchinRippleRadius);
    }
}