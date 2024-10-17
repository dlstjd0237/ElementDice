using UnityEngine;

public class HealDebuff : Debuff
{
    public override void Init()
    {
    }

    public override void Use(Agent agent)
    {
        agent.HealthCompo.ApplyHeal(BaekPlayerManager.Instance.Player.DiceNum, AttackAndTextType.Heal);
        Debug.Log("±â¸ð¶ì");
    }

}
