using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Heart Collection")]
    [SerializeField] private int requiredHearts = 4;
    public UnityEvent<int> onHeartCollected;
    public UnityEvent onAllHeartsCollected;

    private int collectedHearts;
    private bool canBubblePop;

    public bool CanBubblePop => canBubblePop;
    public int CollectedHearts => collectedHearts;

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
    [SerializeField] private SpawnController spawnController;

    public static GameManager instance;
    private bool isRoundActive;
    private PlayerController _winner;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            Debug.LogError("Duplicate game manager detected!");
        }
        instance = this;
        
        // Initialize lists if they're null
        if (playerComponents == null)
            playerComponents = new List<PlayerComponentScriptableObject>();
        if (playerSprites == null)
            playerSprites = new List<Sprite>();
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (player == null) return;
        
        // Safety check for component lists
        if (playerComponents.Count < 2)
        {
            Debug.LogError("Player components not set up in GameManager! Please assign Player1Component and Player2Component in the inspector.");
            return;
        }

        players.Add(player);
        if (players.Count == 1)
        {
            player.SetPlayerType(PlayerType.Bubble);
            player.SetPlayerPosition(spawnController.GetSpawnPoint());
            if (playerSprites.Count > 0)
            {
                playerComponents[0].PlayerSprite = playerSprites[0];
            }
            player.SetPlayerComponent(playerComponents[0]);
            player.name = "Player 1";
            if (player1JoinScreen != null)
            {
                player1JoinScreen.UpdateJoinText();
            }
        }
        else if (players.Count == 2)
        {
            player.SetPlayerType(PlayerType.Urchin);
            player.SetPlayerPosition(spawnController.GetSpawnPoint());
            if (playerSprites.Count > 1)
            {
                playerComponents[1].PlayerSprite = playerSprites[1];
            }
            player.SetPlayerComponent(playerComponents[1]);
            player.name = "Player 2";
            if (player2JoinScreen != null)
            {
                player2JoinScreen.UpdateJoinText();
            }
            if (startButton != null)
            {
                startButton.interactable = true;
                TextMeshProUGUI buttonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
                if (buttonText != null)
                {
                    buttonText.text = "P1 Start";
                }
                EventSystem.current.SetSelectedGameObject(startButton.gameObject);
            }
        }
    }

    public PlayerController GetOtherPlayer(PlayerController self)
    {
        return players.Find(value => value != self);
    }

    public void StartGame()
    {
        players.ForEach(player => player.gameObject.SetActive(false));
        joinScreen.SetActive(false);
        var cinematicOpener = CinematicOpenerController.instance;
        if (cinematicOpener != null) {
            cinematicOpener.OnCinematicOver.AddListener(StartNewRound);
            cinematicOpener.StartCinematic();
        } else {
            Debug.Log("No cinematic opener detected, just starting the round");
            StartNewRound();
        }
    }

    public void StartNewRound()
    {
        players.ForEach(player => player.gameObject.SetActive(true));
        collectedHearts = 0;
        canBubblePop = false;
        Debug.Log("Game Start");
        for (int i = 0; i < players.Count; i++)
        {
            var player = players[i];
            if (player.attributeController != null)
            {
                player.attributeController.ResetAllAttributes();
                player.ApplyPerks();
            }
        }

        if (timer != null)
        {
            timer.gameObject.SetActive(true);
            timer.OnEnd.AddListener(() =>
            {
                var winner = players.Find(player => player.GetPlayerType() == PlayerType.Urchin);
                GameOver(winner);
            });
        }

        isRoundActive = true;
        if (joinScreen != null)
        {
            Destroy(joinScreen);
        }
    }

    public void ModifyRoundTime(float timeChange)
    {
        if (!isRoundActive || timer == null) return;
        timer.ModifyTime(timeChange);
    }

    public void GameOver(PlayerController winner)
    {
        _winner = winner;
        isRoundActive = false;
        if (PerkManager.instance != null)
        {
            PerkManager.instance.LoadPerks(winner);
        }
    }

    public void CollectHeart()
    {
        if (!isRoundActive || canBubblePop) return;

        collectedHearts++;
        onHeartCollected?.Invoke(collectedHearts);

        if (collectedHearts >= requiredHearts)
        {
            canBubblePop = true;
            onAllHeartsCollected?.Invoke();
            var bubblePlayer = players.Find(player => player.GetPlayerType() == PlayerType.Bubble);
            if (bubblePlayer != null)
            {
                bubblePlayer.OnAllHeartsCollected();
            }
        }
    }

    public void BubbleReachedUrchin()
    {
        if (isRoundActive && canBubblePop)
        {
            isRoundActive = false;
            Debug.Log("Bubble wins!");
            var winner = players.Find(player => player.GetPlayerType() == PlayerType.Bubble);
            GameOver(winner);
        }
    }

    public void GiveWinnerAPerk(Perk perk) {
        if (_winner != null) {
            _winner.AddPerk(perk);
            StartNewRound();
        }
    }
}