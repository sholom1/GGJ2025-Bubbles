using UnityEngine;
using System.Collections;

public class SoapEffect : MonoBehaviour, IItemEffect
{
    [Header("Soap Settings")]
    [SerializeField] private float slideSpeed = 8f;
    [SerializeField] private float slipDuration = 2f;
    [SerializeField] private float trapDuration = 3f;
    [SerializeField] private GameObject bubbleTrapPrefab;
    [SerializeField] private int maxBubbleTraps = 3;
    
    [Header("Visual Effects")]
    [SerializeField] private GameObject soapSlidePrefab;
    [SerializeField] private float slideDistance = 3f;
    
    private Rigidbody2D rb;
    private bool isSliding;
    private Vector2 slideDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void ApplyEffect(GameObject collector, GameObject target)
    {
        var playerController = collector.GetComponent<PlayerController>();
        if (playerController == null) return;

        // Get movement direction from player's input
        var movement = collector.GetComponent<Rigidbody2D>().linearVelocity.normalized;
        if (movement.magnitude < 0.1f) 
        {
            // If not moving, use facing direction or default to right
            movement = Vector2.right;
        }

        // Create sliding soap instance
        GameObject slidingSoap = null;
        if (soapSlidePrefab != null)
        {
            slidingSoap = Instantiate(soapSlidePrefab, transform.position, Quaternion.identity);
            StartCoroutine(SlideSoap(slidingSoap, movement));
        }
    }

    private IEnumerator SlideSoap(GameObject soap, Vector2 direction)
    {
        float distanceTraveled = 0;
        Vector2 startPos = soap.transform.position;
        
        // Add collider to detect players
        var collider = soap.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        // Add trigger component to handle player collisions
        var trigger = soap.AddComponent<SoapTrigger>();
        trigger.Initialize(slipDuration, trapDuration, bubbleTrapPrefab, maxBubbleTraps);

        while (distanceTraveled < slideDistance)
        {
            Vector2 newPos = (Vector2)soap.transform.position + direction * slideSpeed * Time.deltaTime;
            soap.transform.position = newPos;
            
            distanceTraveled = Vector2.Distance(startPos, newPos);
            yield return null;

            // Check for walls or obstacles
            var hit = Physics2D.Raycast(soap.transform.position, direction, 0.1f);
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                break;
            }
        }

        // Keep the soap trigger active for a while before destroying
        yield return new WaitForSeconds(5f);
        Destroy(soap);
    }
}

// Helper component to handle soap trigger effects
public class SoapTrigger : MonoBehaviour
{
    private float slipDuration;
    private float trapDuration;
    private GameObject bubbleTrapPrefab;
    private int maxBubbleTraps;
    private int currentBubbleTraps;

    public void Initialize(float slipDuration, float trapDuration, GameObject bubbleTrapPrefab, int maxBubbleTraps)
    {
        this.slipDuration = slipDuration;
        this.trapDuration = trapDuration;
        this.bubbleTrapPrefab = bubbleTrapPrefab;
        this.maxBubbleTraps = maxBubbleTraps;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerController>();
        if (player == null) return;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        // Check if it's the bubble or urchin
        if (IsPlayerType(player, PlayerType.Urchin))
        {
            StartCoroutine(ApplySlipEffect(rb));
        }
        else if (IsPlayerType(player, PlayerType.Bubble) && currentBubbleTraps < maxBubbleTraps)
        {
            CreateBubbleTrap(player.transform.position);
        }
    }

    private bool IsPlayerType(PlayerController controller, PlayerType type)
    {
        var field = typeof(PlayerController).GetField("_type", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (field != null)
        {
            return (PlayerType)field.GetValue(controller) == type;
        }
        return false;
    }

    private IEnumerator ApplySlipEffect(Rigidbody2D rb)
    {
        // Store original values
        float originalDrag = rb.linearDamping;
        float originalAngularDrag = rb.angularDamping;

        // Apply slippery physics
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        rb.AddTorque(5f);

        yield return new WaitForSeconds(slipDuration);

        // Restore original values
        rb.linearDamping = originalDrag;
        rb.angularDamping = originalAngularDrag;
        rb.angularVelocity = 0;
    }

    private void CreateBubbleTrap(Vector3 position)
    {
        if (bubbleTrapPrefab != null && currentBubbleTraps < maxBubbleTraps)
        {
            var trap = Instantiate(bubbleTrapPrefab, position, Quaternion.identity);
            currentBubbleTraps++;
            
            // Destroy trap after duration
            StartCoroutine(DestroyTrapAfterDuration(trap));
        }
    }

    private IEnumerator DestroyTrapAfterDuration(GameObject trap)
    {
        yield return new WaitForSeconds(trapDuration);
        currentBubbleTraps--;
        Destroy(trap);
    }
}