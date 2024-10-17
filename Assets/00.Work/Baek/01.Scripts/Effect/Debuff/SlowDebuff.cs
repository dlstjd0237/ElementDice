using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDebuff : Debuff
{
    private bool _first;
    
    public override void Init()
    {
        base.Init();

        turnCount = 2;
        _effectLevel = 1;
        _debuffDamage = 4;
    }

    public override void Use(Agent agent)
    {
        _debuffDamage = 4;
        agent.HealthCompo.ApplyDamage(_debuffDamage, AttackAndTextType.Slow);
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Slow);

        int randNum = Random.Range(1, 101);
        if (randNum <= 50)
        {
            Vector3 popUpPos = agent.transform.position;
            popUpPos.y += 0.5f;
            PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

            string sendText = "젖음!";
            popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
            
            if(!agent.isWater)
                agent.Stat.damage.SetDefaultValue(agent.Stat.damage.GetValue() / 2);
            agent.isWater = true;
        }
        base.Use(agent);
    }
    
    public override void RemoveProcess(Agent agent)
    {
        agent.Stat.damage.SetDefaultValue(agent.defaultStat.damage.GetValue());
        agent.isWater = false;
        
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 0.5f;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-젖음";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
        base.RemoveProcess(agent);

    }
}
