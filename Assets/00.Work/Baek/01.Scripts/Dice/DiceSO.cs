using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "SO/Baek/Dice/Dice")]
public class DiceSO : ScriptableObject
{
    public string DiceName;
    public DiceSO NextLevelDiceSo;
    public int MaxDiceAmount;
    public Texture Texture;
    public DiceRareType DiceRareType;
    public DiceSidedType DiceSidedType;
    public DiceActionType DiceActionType;
    public DeBuffEffectType DeBuffType;
    public BuffEffectType BuffType;
    public EffectUseNumber EffectNum;
    public Action DiceRollEvent;
    [field: TextArea] public string DiceDescript;
    public int Price
    {

        get
        {

            return 100 * (int)DiceRareType;
        }
        set
        {
            Price = value;
        }

    }


    public List<bool> GetEffectNum()
    {
        List<bool> EffectNumList = new List<bool>();

        Type t = typeof(EffectUseNumber);
        for (int i = 1; i <= 20; ++i) //1~ 20
        {
            FieldInfo field = t.GetField($"Num{i}");
            bool value = (bool)field.GetValue(EffectNum);
            EffectNumList.Add(value);
        }

        return EffectNumList;
    }

}