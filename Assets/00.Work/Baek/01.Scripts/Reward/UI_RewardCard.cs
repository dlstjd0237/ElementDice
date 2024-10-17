using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static Define;
using System;

public class UI_RewardCard : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI _rewardNameText;
    [SerializeField] private Image _rewardIconImage;

    private UI_RewardInfo UI_info;
    private Button _rewardBtn;
    private ERewardType _currentType;

    private int _goldAmount;
    private DiceSO _giveDiceSO;

    private void Awake()
    {
        _rewardBtn = GetComponent<Button>();
        _rewardBtn.onClick.AddListener(HandleAddItem);
    }

    private void HandleAddItem()
    {
        if (_currentType == ERewardType.Gold)
            CurrencyManagerScript.Instance.EarnMoney(_goldAmount);
        else //주사위를 줄 경우
            DiceManager.Instance.AddDiceCardDictionary(_giveDiceSO);

        Destroy(gameObject);
    }

    public void SetInfo(int amount, UI_Clear clearUI)
    {
        _currentType = ERewardType.Gold;
        _goldAmount = amount;
        _rewardNameText.SetText($"{amount} 골드");
        UI_info = clearUI.RewardInfoUI;
    }
    public void SetInfo(DiceSO so, UI_Clear clearUI)
    {
        _currentType = ERewardType.Dice;
        _giveDiceSO = so;
        _rewardNameText.SetText(so.DiceName);
        UI_info = clearUI.RewardInfoUI;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Hover);
        switch (_currentType)
        {
            case ERewardType.Gold:
                UI_info.SetInfo(_goldAmount);
                break;
            case ERewardType.Dice:
                UI_info.SetInfo(_giveDiceSO);
                break;
            default:
                Debug.LogError("ERewardType Error");
                break;
        }
    }
}
