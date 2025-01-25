using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D captureArea;
    [SerializeField]
    private float force = 10f;
    private void OnTriggerStay2D(Collider2D other)
    {
        // If an object that stays in the trigger
        // then apply a force every frame.
        // The force is applied in the direction of the fan
        // Scaled by the players distance from the fan
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Vector2 direction = -transform.right;
        float distance = Vector2.Distance(other.transform.position, transform.position);
        float fanRange = captureArea.transform.localScale.x;
        rb.AddForce(direction * ((fanRange - Mathf.Min(fanRange, distance)) / fanRange) * force, ForceMode2D.Force);
        
    }
}
