using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [Header("Pulse Settings")]
    [SerializeField] private float minScale = 0.8f;
    [SerializeField] private float maxScale = 1.2f;
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private bool useProximitySpeed = false;
    
    private Vector3 originalScale;
    private Transform targetTransform;
    private float currentPulseSpeed;

    private void Start()
    {
        originalScale = transform.localScale;
        currentPulseSpeed = pulseSpeed;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    private void Update()
    {
        if (useProximitySpeed && targetTransform != null)
        {
            float distance = Vector3.Distance(transform.position, targetTransform.position);
            currentPulseSpeed = pulseSpeed + (10f / Mathf.Max(1f, distance));
        }

        float scale = Mathf.Lerp(minScale, maxScale, 
            (Mathf.Sin(Time.time * currentPulseSpeed) + 1f) * 0.5f);
        
        transform.localScale = originalScale * scale;
    }
}