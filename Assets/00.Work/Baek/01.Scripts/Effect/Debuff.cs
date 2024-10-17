using UnityEngine;
using static Define;

public abstract class Debuff : IEffectUseable, IEffectAddble
{
    public int turnCount = 1;
    protected int _effectLevel = 0;
    protected int _debuffDamage = 0;
    protected bool _isStun;
    protected Agent _owner;
    public DeBuffEffectType deBuffEffectType;

    /// <summary>
    ///  세팅값 초기화
    /// </summary>
    public virtual void Init()
    {
        Debug.Log(DiceManager.Instance.CurrentDiceSO.DeBuffType);
        deBuffEffectType = DiceManager.Instance.CurrentDiceSO.DeBuffType;
    }

    public virtual void Use(Agent agent)
    {
        UseEffectiveEffect(agent);
    }

    public virtual void Adding(Agent agent)
    {
        Init();
        agent.EffectiveCompo.AddEffect(this);
    }

    public virtual void RemoveProcess(Agent agent)
    {
        UseRemoveEffect(agent);
    }

    private void UseEffectiveEffect(Agent agent)
    {
        Vector3 spawnPos = agent.transform.position;
        spawnPos.y += 0.6f;
        EffectiveEffect effect = PoolManager.SpawnFromPool<EffectiveEffect>("EffectiveUseEffect", spawnPos);
        effect.Init(ColorManager.Instance.GetDebuffColor(deBuffEffectType));
        effect.Play();
    }
    
    private void UseRemoveEffect(Agent agent)
    {
        Vector3 spawnPos = agent.transform.position;
        EffectiveEffect effect = PoolManager.SpawnFromPool<EffectiveEffect>("EffectiveRemoveEffect", spawnPos);
        effect.Init(ColorManager.Instance.GetDebuffColor(deBuffEffectType));
        effect.Play();
    }
}
