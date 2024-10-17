using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Baek/DiceCardListSO")]
public class DiceCardListSO : ScriptableObject
{
    public List<DiceSO> DiceSOList;
    public Dictionary<string, DiceSO> DiceSODictionary;
    public DiceRareType DiceRare;

    private void OnEnable()
    {
        DiceSODictionary = new Dictionary<string, DiceSO>();
        for (int i = 0; i < DiceSOList.Count; ++i)
        {
            DiceSODictionary.Add(DiceSOList[i].DiceName, DiceSOList[i]);
        }
    }
}
