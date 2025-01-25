using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player Management")]
    [SerializeField] private List<PlayerController> players = new List<PlayerController>();
    [SerializeField] private List<PlayerComponentScriptableObject> playerComponents = new List<PlayerComponentScriptableObject>();
    [SerializeField] private PlayerComponentScriptableObject dummyPlayerComponent;
    [SerializeField] private List<Sprite> playerSprites = new List<Sprite>();
    [SerializeField] private JoinPanel player1JoinScreen;
    [SerializeField] private JoinPanel player2JoinScreen;
    [SerializeField] private GameObject joinScreen;
    [SerializeField] private Button startButton;
    [SerializeField] private Timer timer;

    public static GameManager instance;
    private bool isRoundActive;

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
            player.SetPlayerType(PlayerType.Bubble);
            playerComponents[0].PlayerSprite = playerSprites[0];
            player.SetPlayerComponent(playerComponents.Count > 0 ? playerComponents[0] : dummyPlayerComponent);
            player.name = "Player 1";
            player1JoinScreen.UpdateJoinText();
        }
        else
        {
            player.SetPlayerType(PlayerType.Urchin);
            playerComponents[1].PlayerSprite = playerSprites[1];
            player.SetPlayerComponent(playerComponents.Count > 1 ? playerComponents[1] : dummyPlayerComponent);
            player.name = "Player 2";
            player2JoinScreen.UpdateJoinText();
            startButton.interactable = true;
            startButton.GetComponentInChildren<TextMeshProUGUI>().text = "P1 Start";
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }

    public PlayerController GetOtherPlayer(PlayerController self)
    {
        return players.Find(value => value != self);
    }

    public void StartGame()
    {
        Debug.Log("Game Start");
        timer.gameObject.SetActive(true);
        timer.OnEnd.AddListener(GameOver);
        isRoundActive = true;
        players.ForEach(player => player.GetComponent<HeartBeatRumble>().enabled = true);
        Destroy(joinScreen);
    }

    public void ModifyRoundTime(float timeChange)
    {
        if (!isRoundActive || timer == null) return;
        timer.ModifyTime(timeChange);
    }

    public void GameOver()
    {
        isRoundActive = false;
        SceneManager.LoadScene(0);
    }

    public void BubbleReachedUrchin()
    {
        if (isRoundActive)
        {
            isRoundActive = false;
            // Handle bubble victory logic here
            Debug.Log("Bubble wins!");
            GameOver();
        }
    }
}