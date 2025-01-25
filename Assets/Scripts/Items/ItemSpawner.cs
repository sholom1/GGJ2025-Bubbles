using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private float initialSpawnDelay = 2f;
    [SerializeField] private float spawnInterval = 7f;  // Total time between despawn and next spawn
    [SerializeField] private float minSpawnDelay = 2f;  // Minimum delay after item despawn before next spawn
    
    [Header("Item Settings")]
    [SerializeField] private float itemLifetime = 5f;
    [SerializeField] private float itemFlickerDuration = 2f;
    
    private List<ItemSpawnPoint> spawnPoints;
    private Dictionary<ItemSpawnPoint, Coroutine> activeSpawnRoutines;
    
    private void Awake()
    {
        spawnPoints = new List<ItemSpawnPoint>(GetComponentsInChildren<ItemSpawnPoint>());
        activeSpawnRoutines = new Dictionary<ItemSpawnPoint, Coroutine>();
    }

    private void Start()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            StartSpawnRoutine(spawnPoint);
        }
    }

    private void StartSpawnRoutine(ItemSpawnPoint spawnPoint)
    {
        if (activeSpawnRoutines.ContainsKey(spawnPoint))
        {
            StopCoroutine(activeSpawnRoutines[spawnPoint]);
        }
        
        var routine = StartCoroutine(SpawnRoutine(spawnPoint));
        activeSpawnRoutines[spawnPoint] = routine;
    }

    private IEnumerator SpawnRoutine(ItemSpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(initialSpawnDelay);
        
        while (true)
        {
            // Randomly select and spawn an item
            if (spawnPoint.possibleItems.Length > 0)
            {
                ItemType itemType = spawnPoint.possibleItems[Random.Range(0, spawnPoint.possibleItems.Length)];
                SpawnItem(itemType, spawnPoint.transform.position);
            }

            // Wait for the item lifetime plus a random delay before spawning the next item
            yield return new WaitForSeconds(itemLifetime + itemFlickerDuration);
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, spawnInterval - itemLifetime - itemFlickerDuration));
        }
    }

    private void SpawnItem(ItemType itemType, Vector3 position)
    {
        var itemPrefab = ItemDatabase.Instance.GetItemPrefab(itemType);
        if (itemPrefab != null)
        {
            var item = Instantiate(itemPrefab, position, Quaternion.identity);
            var itemComponent = item.GetComponent<Item>();
            if (itemComponent != null)
            {
                itemComponent.Initialize(itemLifetime, itemFlickerDuration);
            }
        }
    }
}