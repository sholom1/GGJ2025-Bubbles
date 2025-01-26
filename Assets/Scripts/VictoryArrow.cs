using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class VictoryArrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private Color arrowColor = Color.yellow;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private int flashCount = 3;
    [SerializeField] private float rotationSpeed = 90f;

    private SpriteRenderer spriteRenderer;
    private Transform target;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void StartPointingAt(Transform target)
    {
        this.target = target;
        StartCoroutine(FlashArrowSequence());
    }

    private IEnumerator FlashArrowSequence()
    {
        spriteRenderer.enabled = true;
        
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.color = arrowColor;
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.color = new Color(arrowColor.r, arrowColor.g, arrowColor.b, 0f);
            yield return new WaitForSeconds(flashDuration);
        }

        spriteRenderer.color = arrowColor;
        enabled = true; // Start updating rotation
    }

    private void Update()
    {
        if (target != null)
        {
            // Point towards target
            Vector3 direction = target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            
            // Add wobble
            transform.rotation *= Quaternion.Euler(0, 0, 
                Mathf.Sin(Time.time * rotationSpeed) * 15f);
        }
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
    }
}