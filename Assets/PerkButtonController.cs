using TMPro;
using UnityEngine;

public class PerkButtonController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descriptionText;

    private Perk _perk;

    public void SetPerk(Perk perk) {
        _perk = perk;
        nameText.text = perk.Name;
        descriptionText.text = perk.Description;
    }

    public void OnClick() {
        PerkManager.instance.SelectPerk(_perk);
    }
}
