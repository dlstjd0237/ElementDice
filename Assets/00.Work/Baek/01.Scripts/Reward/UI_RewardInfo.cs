using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_RewardInfo : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private TextMeshProUGUI _DescriptionText;
    [SerializeField] private Sprite _diceSprite, _goldSprite;
    public void SetInfo(DiceSO so)
    {
        _iconImage.sprite = _diceSprite;
        _nameText.SetText(so.DiceName);
        _DescriptionText.SetText($"{so.DiceName}¿ª »πµÊ«’¥œ¥Ÿ.");
    }
    public void SetInfo(int amount)
    {
        _iconImage.sprite = _goldSprite;
        _nameText.SetText("∞ÒµÂ");
        _DescriptionText.SetText($"{amount}¿« ∞ÒµÂ∏¶ »πµÊ«’¥œ¥Ÿ.");
    }

}
