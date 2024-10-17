using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BuffEffectColor
{
    public BuffEffectType buffType;
    public Color color;
}

[Serializable]
public struct DeBuffEffectColor
{
    public DeBuffEffectType deBuffType;
    public Color color;
}

public class ColorManager : MonoSingleton<ColorManager>
{
    [SerializeField] private List<BuffEffectColor> _buffTrailColors;
    [SerializeField] private List<DeBuffEffectColor> _deBuffTrailColors;
    
    private Dictionary<BuffEffectType, Color> _buffColorDic = new();
    private Dictionary<DeBuffEffectType, Color> _deBuffColorDic = new();
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        _buffTrailColors.ForEach(x =>
        {
            _buffColorDic.Add(x.buffType, x.color);
        });
        _deBuffTrailColors.ForEach(x =>
        {
            _deBuffColorDic.Add(x.deBuffType, x.color);
        });
    }

    public Color GetDebuffColor(DeBuffEffectType type) => _deBuffColorDic[type];
    public Color GetBuffColor(BuffEffectType type) => _buffColorDic[type];
}
