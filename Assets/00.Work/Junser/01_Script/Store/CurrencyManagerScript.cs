using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CurrencyManagerScript : MonoSingleton<CurrencyManagerScript>
{
    private StoreItemSetting _storeItemSetting;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        _storeItemSetting = FindAnyObjectByType<StoreItemSetting>();
        if (_storeItemSetting != null)
        {
            _storeItemSetting.onPurchase += PurchaseItem;
        }
    }

    private static int currencyGold;
    [SerializeField]
    private TMP_Text _goldUI;
    public int CoinInt { get { return currencyGold; } private set { if (value >= 0) currencyGold = value; } }

    public void SetMoneyText(TMP_Text text)
    {
        _goldUI = text;
        SetCoinUI();
    }
    public void Decrese(int amount)
    {
        CoinInt -= amount;
        SetCoinUI();
    }

    public void EarnMoney(int amount)
    {
        CoinInt += amount;
        SetCoinUI();
    }

    private void SetCoinUI()
    {
        if (_goldUI != null)
        {
            _goldUI.text = CoinInt.ToString();
        }
    }

    public void PurchaseItem(DiceSO item)
    {
        Decrese(item.Price);
    }
}
