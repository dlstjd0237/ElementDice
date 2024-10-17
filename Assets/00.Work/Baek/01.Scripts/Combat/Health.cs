using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _strength = 0.2f;
    private Agent _owner;
    private float _maxHealth;
    private float _currentHealth;
    public float CurrentHealth => _currentHealth;

    private int _damage;
    public void Initialized(Agent owner)
    {
        _owner = owner;
        SetHealth();
    }

    public void ApplyHeal(int healValue, AttackAndTextType attackType)
    {
        _owner.currentDamagedType = attackType;
        _currentHealth = Mathf.Min(_currentHealth + healValue, _maxHealth);
        ShowHealText(attackType, healValue);
    }

    public void ApplyDamage(int damage, AttackAndTextType attackType, Action criticalEvent = null)
    {
        if (attackType == AttackAndTextType.CriticalAttack)
            criticalEvent?.Invoke();

        if (attackType == AttackAndTextType.NormalAttack ||
            attackType == AttackAndTextType.CriticalAttack)
        if (attackType == AttackAndTextType.NormalAttack || attackType == AttackAndTextType.CriticalAttack)
        {
            _damage = _owner.Stat.GetApplyDefenseDamage(damage);
            _owner.Stat.defense.SetDefaultValue(_owner.Stat.defense.GetValue() - damage);

            int afterMinusValue = _owner.Stat.defense.GetValue();
            if (afterMinusValue == 0)
                _owner.AddStatusHUDCompo.RemoveStatus(StatusType.Shield);
            else
                _owner.AddStatusHUDCompo.ChangeStatus(StatusType.Shield,afterMinusValue.ToString());
        }
        else
            _damage = damage;

        _owner.currentDamagedType = attackType;
        _currentHealth = Mathf.Max(_currentHealth - _damage, 0);
        _owner.OnHitEvent?.Invoke();

        ShowDamageText(attackType);
        DamagedCameraFeedback(attackType);
        if (_currentHealth <= 0)
            _owner.OnDeadEvent?.Invoke();
    }

      
    

    public float GetNormalizeHp() => _currentHealth / _maxHealth;

    private void SetHealth()
    {
        _maxHealth = _owner.Stat.GetMaxHealth();
        _currentHealth = _maxHealth;
    }

    private void ShowDamageText(AttackAndTextType type)
    {
        Vector3 popUpPos = transform.position;
        popUpPos.y += 1;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", transform.position);

        if (type == AttackAndTextType.NormalAttack)
            type = _owner.SelfCriticalCheck(_damage) ? AttackAndTextType.CriticalAttack : AttackAndTextType.NormalAttack;

        string sendText = $"-{_damage}";
        popUpText.ShowText(sendText, popUpPos, type);
    }

    private void ShowHealText(AttackAndTextType type, int value)
    {
        Vector3 popUpPos = transform.position;
        popUpPos.y += 1;
        PopUpText popUpText = PoolManager.SpawnFromPool<PopUpText>("PopUpText", transform.position);

        string sendText = $"+{value}";
        popUpText.ShowText(sendText, popUpPos, type);
    }

    private void DamagedCameraFeedback(AttackAndTextType type)
    {
        float duration = _duration;
        float strength = _strength;

        if (_owner.SelfCriticalCheck(_damage))
        {
            duration += 0.15f;
            strength += 0.15f;
        }
        CameraManager.Instance.ShakingCamera(duration, strength);
    }
}
