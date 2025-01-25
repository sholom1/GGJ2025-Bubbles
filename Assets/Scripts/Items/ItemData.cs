using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Bubble Game/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Basic Properties")]
    public ItemType itemType;
    public string itemName;
    [TextArea(3, 5)]
    public string description;
    public Sprite itemSprite;
    
    [Header("Effect Settings")]
    [Tooltip("Duration for temporary effects in seconds")]
    public float effectDuration = 3f;
    
    [Header("Item Settings")]
    public bool destroyOnPickup = true;
}