using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceCard
{
    private ChoiceCheckBoxUI _checkBox;
    private VisualElement _root;
    private Button _cardBtn;
    public Button CardBtn => _cardBtn;
    private ProgressBar _bar;
    private DiceContinViewUI _diceContainViewUI;
    private DiceSO _currentSO;
    public DiceSO CurrentSO
    {
        get => _currentSO;
        set => _currentSO = value;
    }
    private int _amount;
    private int _maxAmount;

    private bool _canLevelUp = false;
    public bool CanLevelUp { get => _canLevelUp; set => _canLevelUp = value; }
    public DiceCard(VisualElement root, DiceSO so, ChoiceCheckBoxUI checkBox, DiceContinViewUI diceContainViewUI)
    {
        _root = root;
        _checkBox = checkBox;
        _diceContainViewUI = diceContainViewUI;
        _currentSO = so;
        _maxAmount = so.MaxDiceAmount;
        _cardBtn = root.Q<Button>("dice_card-contain-btn");
        _bar = root.Q<ProgressBar>("dice_level-progress_bar");
        _amount = 0;
        _bar.value = _amount;
        _bar.highValue = so.MaxDiceAmount;
        _bar.title = $"{_amount}/{_maxAmount}";

        _root.RegisterCallback<ClickEvent>(HandleClickEvent);
        _root.RegisterCallback<MouseEnterEvent>((evt) => SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click));
    }

    private void HandleClickEvent(ClickEvent evt)
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);
        if (_canLevelUp == true)//레벨업 가능 할때 
        {
            _checkBox.SetChoiceBoxHide(false, () =>
             {
                 _diceContainViewUI.AddDice(_currentSO.NextLevelDiceSo);
                 LevelUp();
             });
        }
    }

    public bool AddAmount()
    {
        _amount++;
        _bar.value = _amount;
        if (_amount >= _maxAmount)
        {
            _bar.title = $"{_amount}/{_maxAmount}";
            return true;
        }
        else
        {
            _bar.title = $"{_amount}/{_maxAmount}";
            return false;
        }
    }
    public void LevelUp()
    {
        _canLevelUp = false;
        _amount = 0;
        DiceManager.Instance.DiceAmountDictionary[_currentSO.DiceName] = 0;
        _bar.value = _amount;
        _bar.title = $"{_amount}/{_maxAmount}";

    }

}
