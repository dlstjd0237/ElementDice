using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalysisDebuff : Debuff
{
    public override void Init()
    {
        base.Init();
        
        turnCount = 3;
        _isStun = true;
        _debuffDamage = 5;
    }

    public override void Use(Agent agent)
    {
        _debuffDamage = 5;
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Paralysis);
        int randNum = Random.Range(1, 101);
        if (randNum <= 50)
        {
            Vector3 popUpPos = agent.transform.position;
            popUpPos.y += 0.5f;
            PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

            string sendText = "마비!";
            popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
            agent.isStun = _isStun;
        }
        agent.HealthCompo.ApplyDamage(_debuffDamage, AttackAndTextType.Shock);
        base.Use(agent);
    }

    public override void RemoveProcess(Agent agent)
    {
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 0.5f;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-마비";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
        base.RemoveProcess(agent);

    }
}
