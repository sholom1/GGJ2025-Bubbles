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
        new Perk("Dad's shoes", "Increase the power of your jump.", (player) => player.attributeController.SetJumpForce(1.25f)),
        new Perk("Mom's slippers", "Increase the power of your jump more.", (player) => player.attributeController.SetJumpForce(1.5f)),
        new Perk("New School Shoes", "Get mad jump skills yo!", (player) => player.attributeController.SetJumpForce(2f)),
        new Perk("Leg day gains", "Gain an additional jump.", (player) => player.attributeController.AddExtraJump(1)),
        new Perk("Rayman TM", "Increase your speed", (player) => player.attributeController.SetMovementSpeed(1.1f)),
        new Perk("Sonic TM", "Increase your speed more.", (player) => player.attributeController.SetMovementSpeed(1.2f)),
        new Perk("Flash TM", "Get mad top speed yo!", (player) => player.attributeController.SetMovementSpeed(1.3f)),
        new Perk("Bar Dash", "Increase your dash duration.", (player) => player.attributeController.SetDashDuration(1.05f)),
        new Perk("Door Dash", "Increase your dash duration more.", (player) => player.attributeController.SetDashDuration(1.07f)),
        new Perk("Dine Dash", "Get your dash duration up way high!", (player) => player.attributeController.SetDashDuration(1.09f)),
        new Perk("Pocket Watch", "Decrease your Dash cooldown.", (player) => player.attributeController.SetDashCooldown(0.9f)),
        new Perk("Digital Watch", "Decrease your Dash cooldown more.", (player) => player.attributeController.SetDashCooldown(0.7f)),
        new Perk("Databank Watch", "Get that dash cooldown max upgrade!", (player) => player.attributeController.SetDashCooldown(0.5f)),
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

