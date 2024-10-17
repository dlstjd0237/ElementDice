using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Baek/Dice/ColorList")]
public class DiceColorListSO : ScriptableObject
{
    [SerializeField] private List<DiceColorSO> _diceColorSOList; public List<DiceColorSO> DiceColorSOList { get { return _diceColorSOList; } }
}
