using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private List<PlayerController> players = new List<PlayerController>();
    [SerializeField]
    private List<PlayerComponentScriptableObject> playerComponents = new List<PlayerComponentScriptableObject>();
    [SerializeField]
    private PlayerComponentScriptableObject dummyPlayerComponent;
    [SerializeField]
    private List<Sprite> playerSprites = new List<Sprite>();
    [SerializeField]
    private JoinPanel player1JoinScreen;
    [SerializeField]
    private JoinPanel player2JoinScreen;
    [SerializeField]
    private GameObject joinScreen;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Timer timer;
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
            // player.SetPlayerType(PlayerType.Bubble);
            playerComponents[0].PlayerSprite = playerSprites[0];
            player.SetPlayerComponent(playerComponents.Count > 0 ? playerComponents[0] : dummyPlayerComponent);
            player1JoinScreen.UpdateJoinText();
        }
        else
        {
            //Player is urchin
            // player.SetPlayerType(PlayerType.Urchin);
            playerComponents[1].PlayerSprite = playerSprites[1];
            player.SetPlayerComponent(playerComponents.Count > 1 ? playerComponents[1] : dummyPlayerComponent);

            player2JoinScreen.UpdateJoinText();
            startButton.interactable = true;
            startButton.GetComponentInChildren<TextMeshProUGUI>().text = "P1 Start";
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }
    }
    public void StartGame()
    {
        Debug.Log("Game Start");
        timer.gameObject.SetActive(true);
        timer.OnEnd.AddListener(GameOver);
        Destroy(joinScreen);
    }
    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }
}
