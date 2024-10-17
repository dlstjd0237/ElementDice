using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Junser/Item")]
public class ItemSO : ScriptableObject
{
    //아이템이 가지고 있을 요소
    public string _itemName;
    public string _itemTooltip;
    public Sprite _itemImage;
    public Color _color;
    public int _cost;
}
