using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Item : MonoBehaviour
{
    [Header("Item Configuration")]
    [SerializeField] private ItemData itemData;
    
    [Header("Events")]
    public UnityEvent<GameObject, GameObject> onPickupByBubble;
    public UnityEvent<GameObject, GameObject> onPickupByUrchin;
    public UnityEvent onDestroy;

    private SpriteRenderer spriteRenderer;
    private float spawnTime;
    private float lifetime;
    private float flickerDuration;
    private bool isFlickering;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Log($"Item Awake - GameObject: {gameObject.name}, Has SpriteRenderer: {spriteRenderer != null}");
        InitializeItem();
    }

    public void Initialize(float lifetime, float flickerDuration)
    {
        this.lifetime = lifetime;
        this.flickerDuration = flickerDuration;
        spawnTime = Time.time;
        StartCoroutine(LifetimeRoutine());
    }

    private void InitializeItem()
    {
        if (itemData == null)
        {
            Debug.LogError($"ItemData not assigned to item: {gameObject.name}");
            return;
        }

        Debug.Log($"Initializing item {gameObject.name} - ItemData: {itemData.name}, Sprite: {itemData.itemSprite != null}");

        if (spriteRenderer != null && itemData.itemSprite != null)
        {
            spriteRenderer.sprite = itemData.itemSprite;
            Debug.Log($"Sprite assigned to {gameObject.name}");
        }
        else
        {
            Debug.LogError($"Failed to set sprite - SpriteRenderer: {spriteRenderer != null}, ItemData Sprite: {itemData.itemSprite != null}");
        }
    }

    private IEnumerator LifetimeRoutine()
    {
        // Wait for the main lifetime
        yield return new WaitForSeconds(lifetime);
        
        // Start flickering
        isFlickering = true;
        StartCoroutine(FlickerRoutine());
        
        // Wait for flicker duration then destroy
        yield return new WaitForSeconds(flickerDuration);
        DestroyItem();
    }

    private IEnumerator FlickerRoutine()
    {
        float flickerRate = 0.1f;
        while (isFlickering)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flickerRate);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController collector = other.GetComponent<PlayerController>();
            if (collector != null)
            {
                HandlePickup(collector);
            }
        }
    }

    private void HandlePickup(PlayerController collector)
    {
        if (isFlickering) return; // Can't pick up while flickering
        
        // Find the other player (target)
        GameObject target = FindOtherPlayer(collector.gameObject);
        if (target == null) return;

        // Invoke the appropriate event
        if (collector.CompareTag("Bubble"))
        {
            onPickupByBubble?.Invoke(collector.gameObject, target);
        }
        else
        {
            onPickupByUrchin?.Invoke(collector.gameObject, target);
        }

        if (itemData.destroyOnPickup)
        {
            DestroyItem();
        }
    }

    private GameObject FindOtherPlayer(GameObject collector)
    {
        string targetTag = collector.CompareTag("Bubble") ? "Urchin" : "Bubble";
        return GameObject.FindGameObjectWithTag(targetTag);
    }

    public void DestroyItem()
    {
        isFlickering = false;
        onDestroy?.Invoke();
        Destroy(gameObject);
    }

    public ItemData GetItemData() => itemData;
}