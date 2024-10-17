using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "SO/Junser/Item")]
public class ItemSO : ScriptableObject
{
    //�������� ������ ���� ���
    public string _itemName;
    public string _itemTooltip;
    public Sprite _itemImage;
    public Color _color;
    public int _cost;
}
