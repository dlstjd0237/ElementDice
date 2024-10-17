using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    private int _mulValue = 1;
    public override void Init()
    {
        base.Init();
        
        _mulValue = 1;
        _debuffDamage = 3;
        _effectLevel = 1;
        turnCount = 3;
        _isStun = false;
    }

    public override void Use(Agent agent)
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Addiction);
        _debuffDamage = (agent.Stat.GetMaxHealth() / 16 * _mulValue);
        agent.HealthCompo.ApplyDamage(_debuffDamage, AttackAndTextType.Poison);
        _mulValue++;
        base.Use(agent);
    }
    
    public override void RemoveProcess(Agent agent)
    {
        _mulValue = 1;
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 0.5f;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-독";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
        base.RemoveProcess(agent);

    }
}
