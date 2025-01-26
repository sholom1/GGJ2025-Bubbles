using UnityEngine;

[RequireComponent(typeof(SoapEffect))]
public class SoapItem : Item
{
    private SoapEffect soapEffect;

    protected override void Awake()
    {
        base.Awake();
        soapEffect = GetComponent<SoapEffect>();
        
        // Subscribe to pickup events
        onPickupByBubble.AddListener(soapEffect.ApplyEffect);
        onPickupByUrchin.AddListener(soapEffect.ApplyEffect);
    }
}