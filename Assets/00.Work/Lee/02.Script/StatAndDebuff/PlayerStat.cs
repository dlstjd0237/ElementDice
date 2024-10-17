using System;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/InHae/Stat/Player")]
public class PlayerStat : AgentStat
{
    protected override void OnEnable()
    {
        base.OnEnable();

        Type playerStatType = GetType();

        foreach (StatType statType in Enum.GetValues(typeof(StatType)))
        {
            string fieldName = LowerFirstChar(statType.ToString()); //enum의 앞글자만 소문자로 변경

            try
            {
                FieldInfo playerStatField = playerStatType.GetField(fieldName);
                Stat stat = playerStatField.GetValue(this) as Stat;
                _statDictionary.Add(statType, stat);
            }
            catch (Exception e)
            {
                Debug.LogError($"There are no stat filed in player : {fieldName}, msg: {e.Message}");
            }
        }
    }

    public Stat GetStatByType(StatType statType)
    {
        return _statDictionary[statType];
    }

    private string LowerFirstChar(string input)
    {
        return char.ToLower(input[0]) + input.Substring(1);
    }

    public override int GetMeleeDamage() => BaekPlayerManager.Instance.Player.DiceNum;
}