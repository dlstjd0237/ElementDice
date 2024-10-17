using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using static Define;

using Random = UnityEngine.Random;

public class StoreItemSetting : MonoBehaviour
{
    public DiceCardListSO items;
    public Dictionary<Button, Purchase> store = new Dictionary<Button, Purchase>();
    [SerializeField]
    private Inventory inven;
    private Purchase[] purchase;
    private Button[] buttons;

    [SerializeField]
    private Vector3 moveEffectSize;

    [SerializeField]
    private Button _resetButton;

    public event Action<DiceSO> onPurchase;
    //아이템은 중복 가능

    [SerializeField]
    private GameObject panal_ThereIsNoEnoughMoney, panal_ThereIsNoEnoughSpace;
    private void OnEnable()
    {
        buttons = GetComponentsInChildren<Button>();
        purchase = GetComponentsInChildren<Purchase>();
        int i = 0;

        foreach (Button button in buttons)//버튼에 Dictionary 할당, AddListener추가
        {
            store.Add(button, purchase[i]);

            button.onClick.AddListener(() =>
            {
                SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
                PurchaseItem(button);
                StartCoroutine(WaitForPurchase());
            });
            i++;
        }
        ItemSet();
    }

    private IEnumerator WaitForPurchase()
    {
        _resetButton.interactable = false;
        yield return new WaitForSeconds(2);
        _resetButton.interactable = true;
    }


    public void ItemSet()
    {
        for (int j = 0; j < purchase.Length; j++)
        {
            DiceSO itemTemp = items.DiceSOList[Random.Range(0, items.DiceSOList.Count)];
            purchase[j].StoreItem = itemTemp;
        }
        foreach (Button button in buttons)
        {
            button.interactable = true;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("구매");
        }
    }

    public void PurchaseItem(Button button)
    {
        int Price = store[button].StoreItem.Price;
        if (inven.items.Count >= inven.SlotCount)
        {
            panal_ThereIsNoEnoughSpace.SetActive(true);
            print("공간이 없습니다");
        }
        else if (CurrencyManagerScript.Instance.CoinInt < Price)
        {
            panal_ThereIsNoEnoughMoney.SetActive(true);
            print("돈이 없습니다.");
        }
        else
        {
            CurrencyManagerScript.Instance.Decrese(Price);
            onPurchase?.Invoke(store[button].StoreItem);
            button.interactable = false;
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("이미 구매함");
            DiceManager.Instance.AddDiceCardDictionary(store[button].StoreItem);
            PurchaseEffect(button);
        }
    }
    private void PurchaseEffect(Button button)
    {
        RectTransform rectTransform = button.transform.parent.GetComponent<RectTransform>();
        Vector3 currentPos = rectTransform.anchoredPosition;

        Vector3 targetPos = currentPos + moveEffectSize;
        rectTransform.DOAnchorPos(targetPos, 1.0f).SetEase(Ease.InOutBack).SetLoops(2, LoopType.Yoyo);

    }

}
