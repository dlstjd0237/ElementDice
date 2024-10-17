using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBuff : Buff
{
    public override void Init()
    { 
        base.Init();
        turnCount = 1;
    }

    public override void Use(Agent agent)
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Heal);
        _buffValue = BaekPlayerManager.Instance.Player.DiceNum;
        agent.HealthCompo.ApplyHeal(_buffValue, AttackAndTextType.Heal);
        agent.HealthBarCompo.HandleChangeHp();
    }
}
