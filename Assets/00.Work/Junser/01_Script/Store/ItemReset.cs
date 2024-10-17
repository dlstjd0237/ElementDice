using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemReset : MonoBehaviour
{
    private Button resetButton;
    [SerializeField]
    private int cost;
    [SerializeField]
    private GameObject _thereIsNoEnoughMoney;

    [SerializeField]
    private GameObject _shopitems;

    private StoreItemSetting _storeItemSetting;

    private TextMeshProUGUI _resetTxt;


    [SerializeField]
    private Vector3 moveEffectSize;
    private void Awake()
    {
        _resetTxt = GetComponentInChildren<TextMeshProUGUI>();
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(() =>
        {
            SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
            if (CurrencyManagerScript.Instance.CoinInt - cost >= 0)
            {
                StartCoroutine(SetItemDuration());
                PurchaseEffect(_shopitems);
            }
            else
            {
                _thereIsNoEnoughMoney.SetActive(true);
            }
            SetTxt();
        });
        SetTxt();
    }
    private void PurchaseEffect(GameObject items)
    {
        RectTransform rectTransform = items.GetComponent<RectTransform>();
        Vector3 currentPos = rectTransform.anchoredPosition;
        print(currentPos);

        Vector3 targetPos = currentPos + moveEffectSize;
        rectTransform.DOAnchorPos(targetPos, 1.0f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);

    }

    private IEnumerator SetItemDuration()
    {
        resetButton.interactable = false;
        yield return new WaitForSeconds(1f);
        _storeItemSetting = FindAnyObjectByType<StoreItemSetting>();
        _storeItemSetting.ItemSet();
        CurrencyManagerScript.Instance.Decrese(cost);
        cost += 5;
        yield return new WaitForSeconds(1f);
        resetButton.interactable = true;
    }

    private void SetTxt()
    {
        _resetTxt.text = $"새로고침 : {cost}";
    }
}
