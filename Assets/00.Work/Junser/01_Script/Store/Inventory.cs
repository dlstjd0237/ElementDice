using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Inventory : MonoBehaviour
{
    public List<DiceSO> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private InventorySlot[] slots;

    public int SlotCount { get; private set; }

    private int currentGold = 0;

    private StoreItemSetting _storeItemSetting;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<InventorySlot>();
    }
#endif

    private void Awake()
    {
        FreshSlot();
        SlotCount = slots.Length;
        _storeItemSetting = FindAnyObjectByType<StoreItemSetting>();
    }
    private void Start()
    {
        _storeItemSetting.onPurchase += AddItem;
    }

    private void FreshSlot()
    {
        int i = 0;
        for (; i < items.Count; i++)
        {
            slots[i].Item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].Item = null;
        }
    }

    public void AddItem(DiceSO item)
    {
        if (items.Count < slots.Length)
        {
            items.Add(item);
            FreshSlot();
        }
        else
        {
            
        }
    }
}
