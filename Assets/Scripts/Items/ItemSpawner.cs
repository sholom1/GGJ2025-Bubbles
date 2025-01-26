using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float initialSpawnDelay = 2f;
    [SerializeField] private float maxSpawnDelay = 7f;  // Maximum time to wait before spawning next item
    [SerializeField] private float minSpawnDelay = 2f;  // Minimum time to wait before spawning next item
    
    [Header("Item Settings")]
    [SerializeField] private float itemLifetime = 5f;
    [SerializeField] private float itemFlickerDuration = 2f;
    
    private List<ItemSpawnPoint> spawnPoints;
    private Dictionary<ItemSpawnPoint, SpawnPointState> spawnPointStates;
    
    private class SpawnPointState
    {
        public Coroutine activeRoutine;
        public GameObject currentItem;
    }
    
    private void Awake()
    {
        spawnPoints = new List<ItemSpawnPoint>(GetComponentsInChildren<ItemSpawnPoint>());
        spawnPointStates = new Dictionary<ItemSpawnPoint, SpawnPointState>();
        
        foreach (var spawnPoint in spawnPoints)
        {
            spawnPointStates[spawnPoint] = new SpawnPointState();
        }
    }

    private void Start()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            // Add random initial delay for each spawn point
            float initialRandomDelay = Random.Range(0f, maxSpawnDelay);
            StartCoroutine(InitialSpawnDelay(spawnPoint, initialRandomDelay));
        }
    }

    private IEnumerator InitialSpawnDelay(ItemSpawnPoint spawnPoint, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartSpawnRoutine(spawnPoint);
    }

    private void StartSpawnRoutine(ItemSpawnPoint spawnPoint)
    {
        var state = spawnPointStates[spawnPoint];
        
        if (state.activeRoutine != null)
        {
            StopCoroutine(state.activeRoutine);
        }
        
        state.activeRoutine = StartCoroutine(SpawnRoutine(spawnPoint));
    }

    private IEnumerator SpawnRoutine(ItemSpawnPoint spawnPoint)
    {
        var state = spawnPointStates[spawnPoint];
        yield return new WaitForSeconds(initialSpawnDelay);
        
        while (true)
        {
            // Clear any destroyed items from the state
            if (state.currentItem == null)
            {
                state.currentItem = null;
            }
            
            // Only spawn if there's no active item
            if (state.currentItem == null && spawnPoint.possibleItems.Length > 0)
            {
                // Random chance to spawn an item
                if (Random.value > 0.3f) // 70% chance to spawn
                {
                    ItemType itemType = spawnPoint.possibleItems[Random.Range(0, spawnPoint.possibleItems.Length)];
                    state.currentItem = SpawnItem(itemType, spawnPoint.transform.position);
                    
                    // Wait for the full item lifetime
                    yield return new WaitForSeconds(itemLifetime + itemFlickerDuration);
                }
            }

            // Random delay before next spawn attempt
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    private GameObject SpawnItem(ItemType itemType, Vector3 position)
    {
        var itemPrefab = ItemDatabase.Instance.GetItemPrefab(itemType);
        if (itemPrefab != null)
        {
            // Ensure Z position is always 0
            Vector3 spawnPosition = new Vector3(position.x, position.y, 0f);
            var item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            var itemComponent = item.GetComponent<Item>();
            if (itemComponent != null)
            {
                itemComponent.Initialize(itemLifetime, itemFlickerDuration);
                
                // Subscribe to the item's destroy event
                itemComponent.onDestroy.AddListener(() => {
                    // Find and clear the spawn point state when item is destroyed
                    var spawnPoint = spawnPoints.Find(sp => Vector3.Distance(sp.transform.position, position) < 0.1f);
                    if (spawnPoint != null && spawnPointStates.ContainsKey(spawnPoint))
                    {
                        spawnPointStates[spawnPoint].currentItem = null;
                    }
                });
            }
            return item;
        }
        return null;
    }
}