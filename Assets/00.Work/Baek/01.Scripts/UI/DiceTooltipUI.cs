using UnityEngine;
using TMPro;
public class DiceTooltipUI : MonoBehaviour
{
    private TextMeshProUGUI _tooltipTxt;

    private void Awake()
    {
        _tooltipTxt = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        DiceManager.Instance.ChangeDiceEvent += HandleDiceChangeEvent;
    }

    private void HandleDiceChangeEvent(DiceSO dice)
    {
        _tooltipTxt.SetText(dice.DiceDescript);
    }
    private void OnDisable()
    {
        DiceManager.Instance.ChangeDiceEvent -= HandleDiceChangeEvent;
    }
}
