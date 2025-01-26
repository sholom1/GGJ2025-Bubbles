using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    [SerializeField] private Transform perkSelectCanvas;
    [SerializeField] private Transform perkSelectGrid;
    [SerializeField] private Transform winnerText;

   public static PerkManager instance;

   private List<Perk> availablePerks = new List<Perk>() {
        new Perk("Jump force", (player) => player.jumpForce += 1),
        new Perk("Jump force +", (player) => player.jumpForce += 3),
        new Perk("Jump force ++", (player) => player.jumpForce += 5),
   };

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance.gameObject);
            Debug.LogError("Duplicate perk manager detected!");
        }
        instance = this;
    }

    public void LoadPerks(PlayerController winner) 
    {
        winnerText.GetComponent<TextMeshProUGUI>().text = winner.GetPlayerType() + " wins!";
        for (int i = 0; i < perkSelectGrid.childCount; i++)
        {
            var child = perkSelectGrid.GetChild(i);
            var perk = GetRandomPerk();
            child.GetComponent<PerkButtonController>()
                .SetPerk(perk);
        }

        Canvas canvas = perkSelectCanvas.GetComponent<Canvas>();
        canvas.enabled = true;
    }

    public void SelectPerk(Perk perk) {
        GameManager.instance.GiveWinnerAPerk(perk);
        perkSelectCanvas.GetComponent<Canvas>().enabled = false;
    }

    private Perk GetRandomPerk() {
        var randomIndex = UnityEngine.Random.Range(0, availablePerks.Count);
        var perk = availablePerks[randomIndex];
        availablePerks.RemoveAt(randomIndex);

        return perk;
    }
}

