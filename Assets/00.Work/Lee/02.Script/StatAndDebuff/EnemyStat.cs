using UnityEngine;

[CreateAssetMenu(menuName = "SO/InHae/Stat/Enemy")]
public class EnemyStat : AgentStat
{
    [Header("Level Detail")] [SerializeField]
    private int _level;

    [Range(0, 1f)] [SerializeField] private float _percentageModifier;

    public void Modify(Stat stat)
    {
        for (int i = 1; i < _level; i++)
        {
            float modifier = stat.GetValue() * _percentageModifier;
            stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }
}