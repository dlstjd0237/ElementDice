using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Clear : UI_Popup
{
    [SerializeField]
    private UI_RewardCard _rewardPrefab;
    [SerializeField]
    private Transform _rewardRoot;

    [SerializeField]
    private UI_RewardInfo _rewardInfoUI;
    public UI_RewardInfo RewardInfoUI => _rewardInfoUI;

    [SerializeField]
    private DiceCardListSO _cardList;

    [SerializeField]
    private string _mapSceneName;

    [SerializeField]
    private Button _mapBtn, _exit;


    private void OnEnable()
    {
        BattleManager.Instance.ClearEvent.AddListener(OpenWindow);
        _mapBtn.onClick.AddListener(HandleMapClick);
        _exit.onClick.AddListener(HandleExit);
    }

    private void HandleExit()
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);
        SceneControlManager.FadeOut(() => Application.Quit());

    }

    private void HandleMapClick()
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);
        SceneControlManager.FadeOut(() => SceneManager.LoadScene(_mapSceneName));
    }

    private void OnDisable()
    {
        BattleManager.Instance.ClearEvent.RemoveAllListeners();
        _mapBtn.onClick.RemoveListener(HandleMapClick);
        _exit.onClick.RemoveListener(HandleExit);

    }

    public void OpenWindow(/*List<DiceSO> diceList, int goldAmount = 0*/)
    {
        ActiveWindow(true);

        int goldAmount = Random.Range(100, 500);


        if (goldAmount != 0)
        {
            CreateReward(goldAmount);
        }

        int RandomCount = Random.Range(0, 6);

        for (int i = 0; i < RandomCount; ++i)
        {
            CreateReward(_cardList.DiceSOList[Random.Range(0, _cardList.DiceSOList.Count)]);
        }

    }
    private void CreateReward(DiceSO so)
    {
        var obj = Instantiate(_rewardPrefab, _rewardRoot);
        obj.SetInfo(so, this);


    }
    private void CreateReward(int amount)
    {
        var obj = InstantiateReward();
        obj.SetInfo(amount, this);
    }

    private UI_RewardCard InstantiateReward()
     => Instantiate(_rewardPrefab, _rewardRoot);

}
