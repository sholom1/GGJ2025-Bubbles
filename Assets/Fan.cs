using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D captureArea;
    [SerializeField]
    private float force = 10f;
    private void OnTriggerStay2D(Collider2D other)
    {
        // If the object that enters the trigger is the player
        // then apply a force to the player
        // The force is applied in the direction of the fan
        // Scaled by the players distance from the fan
        
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        Vector2 direction = -transform.right; //(other.transform.position - transform.position).normalized;
            float distance = Vector2.Distance(other.transform.position, transform.position);
        float fanRange = captureArea.transform.localScale.x;// * transform.localScale.x;
            Debug.Log("Distance: " + distance);
            Debug.Log("Fan Range: " + fanRange);
            rb.AddForce(direction * ((fanRange - Mathf.Min(fanRange, distance)) / fanRange) * force, ForceMode2D.Force);
        
    }
}
