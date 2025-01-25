using UnityEngine;

public class ItemSpawnPoint : MonoBehaviour
{
    public ItemType[] possibleItems;  // Items that can spawn at this point
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}