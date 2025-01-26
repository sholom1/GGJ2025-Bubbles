using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    private void Awake()
    {
        if (spawnPoints.Count <= 0)
        {
            return;
        }
        
        ShuffleList(spawnPoints);
    }

    public Vector2 GetSpawnPoint()
    {
        if (spawnPoints.Count <= 0)
        {
            Vector2 spawnPoint = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(3.0f, 5.0f));
            return spawnPoint;
        }
        
        Transform random = spawnPoints[Random.Range(0, spawnPoints.Count)];
        spawnPoints.Remove(random);
        return random.position;
    }
    
    void ShuffleList(List<Transform> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
    }
}