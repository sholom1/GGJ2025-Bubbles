using TMPro;
using UnityEngine;

public class PerkButtonController : MonoBehaviour
{
    private Perk _perk;

    public void SetPerk(Perk perk) {
        _perk = perk;
        GetComponentInChildren<TextMeshProUGUI>().text = perk.Name;
    }

    public void OnClick() {
        PerkManager.instance.SelectPerk(_perk);
    }
}
