using UnityEngine;

public class SlashFeedback : Feedback
{
    [SerializeField] private SlashAnimation _slashAnimation;

    public override void CreateFeedback()
    {
        _slashAnimation.gameObject.SetActive(true);
        _slashAnimation.SetEffect(_agent.EnemyCriticalCheck(_agent.Stat.GetMeleeDamage()), 1);
        _slashAnimation.PlayEffect();
    }

    public override void FinishFeedback()
    {
        _slashAnimation.gameObject.SetActive(false);
    }
}
