using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseBuff : Buff
{
    public override void Init()
    {
        base.Init();
        _buffValue = 3;
        turnCount = 1;
    }

    public override void Use(Agent agent)
    {
        _buffValue = 5;
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Defense);
        agent.Stat.defense.SetDefaultValue(agent.Stat.defense.GetValue() + _buffValue);
        
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 1;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = $"+{_buffValue}";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.Defense);
        
        agent.AddStatusHUDCompo.AddStatus(StatusType.Shield, _buffValue.ToString());
    }
    public override void RemoveProcess(Agent agent)
    {
        agent.Stat.defense.SetDefaultValue(agent.Stat.defense.GetValue() - _buffValue);
        
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 1;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-방패";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.Defense);
        
        agent.AddStatusHUDCompo.RemoveStatus(StatusType.Shield);
    }
}
