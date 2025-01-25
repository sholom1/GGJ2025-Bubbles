using UnityEngine;

[RequireComponent(typeof(Item))]
public class ClockItemEffect : MonoBehaviour, IItemEffect
{
    [SerializeField] private float timeChangeAmount = 5f;  // Amount of time to add/subtract
    
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();

        if (item == null)
        {
            Debug.LogError("ClockItemEffect requires an Item component!");
            return;
        }

        // Subscribe to pickup events
        item.onPickupByBubble.AddListener((collector, target) => ApplyEffect(collector, target));
        item.onPickupByUrchin.AddListener((collector, target) => ApplyEffect(collector, target));
    }

    public void ApplyEffect(GameObject collector, GameObject target)
    {
        // If bubble collects, add time. If urchin collects, subtract time
        float timeModifier = collector.CompareTag("Bubble") ? timeChangeAmount : -timeChangeAmount;
        
        // Modify the game timer using the singleton instance
        GameManager.instance.ModifyRoundTime(timeModifier);
    }
}