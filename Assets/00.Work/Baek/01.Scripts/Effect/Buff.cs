using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : IEffectUseable, IEffectAddble
{
    protected int _effectLevel = 0;
    protected int _buffValue = 0;
    protected bool _isInvincibility;
    protected Agent _owner;
    public int turnCount = 1;
    public BuffEffectType buffEffectType;

    /// <summary>
    ///  세팅값 초기화
    /// </summary>
    public virtual void Init()
    {
        buffEffectType = DiceManager.Instance.CurrentDiceSO.BuffType;
    }
    public abstract void Use(Agent agent);

    public virtual void Adding(Agent agent)
    {
        Init();
        agent.EffectiveCompo.AddEffect(this);
    }

    public virtual void RemoveProcess(Agent agent)
    {
        Debug.Log("SADdfasdfsdf");
    }
}
