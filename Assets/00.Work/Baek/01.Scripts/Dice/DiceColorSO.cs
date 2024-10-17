using UnityEngine;

[CreateAssetMenu(menuName = "SO/Baek/Dice/Color")]
public class DiceColorSO : ScriptableObject
{
    [SerializeField] private DiceRareType _diceRareType; public DiceRareType DiceRareType { get { return _diceRareType; } }
    [SerializeField] private Color _color; public Color Color { get { return _color; } }
}
