using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] private List<Vector2> spawnPoints;

    private void Awake()
    {
        ShuffleList(spawnPoints);
    }

    public Vector2 GetSpawnPoint()
    {
        if (spawnPoints.Count <= 0)
        {
            Vector2 spawnPoint = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(3.0f, 5.0f));
            return spawnPoint;
        }
        
        Vector2 random = spawnPoints[Random.Range(0, spawnPoints.Count)];
        spawnPoints.Remove(random);
        return random;
    }
    
    void ShuffleList(List<Vector2> list)
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