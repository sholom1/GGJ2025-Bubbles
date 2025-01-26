using UnityEngine;

[RequireComponent(typeof(PowerUpEffect))]
public class PowerUpItem : Item
{
    private PowerUpEffect powerUpEffect;

    protected override void Awake()
    {
        base.Awake();  // Call base class Awake first
        powerUpEffect = GetComponent<PowerUpEffect>();
        
        // Subscribe to pickup events
        onPickupByBubble.AddListener(powerUpEffect.ApplyEffect);
        onPickupByUrchin.AddListener(powerUpEffect.ApplyEffect);
    }
}