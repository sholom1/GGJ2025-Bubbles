using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerController> players = new List<PlayerController>();
    public static GameManager instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            Debug.LogError("Duplicate game manager detected!");
        }
        instance = this;
    }
    public void RegisterPlayer(PlayerController player)
    {
        players.Add(player);
        if (players.Count == 1)
        {
            //Player is bubble
            player.SetPlayerType(PlayerType.Bubble);
        }
        else
        {
            //Player is urchin
            player.SetPlayerType(PlayerType.Urchin);
        }
    }
}
