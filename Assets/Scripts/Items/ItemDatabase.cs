using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    [System.Serializable]
    public class ItemPrefabEntry
    {
        public ItemType itemType;
        public GameObject prefab;
    }

    [SerializeField] private List<ItemPrefabEntry> itemPrefabs;
    private Dictionary<ItemType, GameObject> prefabLookup;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializePrefabDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePrefabDictionary()
    {
        prefabLookup = new Dictionary<ItemType, GameObject>();
        foreach (var entry in itemPrefabs)
        {
            prefabLookup[entry.itemType] = entry.prefab;
        }
    }

    public GameObject GetItemPrefab(ItemType itemType)
    {
        if (prefabLookup.TryGetValue(itemType, out GameObject prefab))
        {
            return prefab;
        }
        
        Debug.LogWarning($"No prefab found for item type: {itemType}");
        return null;
    }
}