using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostbiteDebuff : Debuff
{
    public override void Init()
    {
        deBuffEffectType = DeBuffEffectType.Frostbite;
        
        turnCount = 2;
        _effectLevel = 1;
        _isStun = true;
    }

    public override void Use(Agent agent)
    {
        agent.currentDamagedType = AttackAndTextType.Frost;
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Frostbite);
        agent.OnHitEvent?.Invoke();     
        
        int randNum = Random.Range(1, 101);
        if (randNum <= 50)
        {
            Vector3 popUpPos = agent.transform.position;
            popUpPos.y += 0.5f;
            PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

            string sendText = "동상!";
            popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
            agent.isStun = _isStun;
        }

        base.Use(agent);
    }
    
    public override void RemoveProcess(Agent agent)
    {
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 0.5f;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-동결";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
        base.RemoveProcess(agent);

    }
}
