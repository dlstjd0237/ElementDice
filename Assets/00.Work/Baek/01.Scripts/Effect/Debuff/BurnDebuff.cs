using UnityEngine;

public class BurnDebuff : Debuff
{
    public override void Init()
    {
        base.Init();
        _debuffDamage = 7;
        _effectLevel = 1;
        turnCount = 3;
        _isStun = false;
    }

    public override void Use(Agent agent)
    {
        _debuffDamage = 7;
        agent.HealthCompo.ApplyDamage(_debuffDamage, AttackAndTextType.Burn);
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Burn);
        Debug.Log("번 데미지 받았음");
        base.Use(agent);

    }
    
    public override void RemoveProcess(Agent agent)
    {
        Vector3 popUpPos = agent.transform.position;
        popUpPos.y += 0.5f;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", agent.transform.position);

        string sendText = "-화염";
        popUpText.ShowText(sendText,popUpPos, AttackAndTextType.DefaultText);
        
        base.RemoveProcess(agent);
    }
}
