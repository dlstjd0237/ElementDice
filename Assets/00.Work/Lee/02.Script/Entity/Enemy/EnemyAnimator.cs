using System;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour, IEnemyComponent
{
    private readonly int _deadHash = Animator.StringToHash("isDead");
    private readonly int _runHash = Animator.StringToHash("isRun");
    private readonly int _attackTriggerHash = Animator.StringToHash("Attack");
    private readonly int _damagedTriggerHash = Animator.StringToHash("Damaged");
    private readonly int _stunHash = Animator.StringToHash("isStun");

    private Enemy _enemy;
    private Animator _animator;

    public void Initialize(Enemy enemy)
    {
        _enemy = enemy;
        _animator = _enemy.AnimatorCompo;
        
        _enemy.DeadEvent+=HandleDeadEvent;
        _enemy.OnHitEvent.AddListener(HandleDamagedEvent);

        var attackCompo = _enemy.GetCompo<EnemyAttack>();
        attackCompo.AttackEvent += HandleAttackEvent;
        attackCompo.RunEvent += HandleRunnerEvent;
    }

    private void HandleRunnerEvent(bool isActive)
    {
        _animator.SetBool(_runHash, isActive);
    }

    private void HandleAttackEvent()
    {
        _animator.SetTrigger(_attackTriggerHash);
    }

    private void HandleDeadEvent(bool isDead)
    {
        _animator.SetBool(_deadHash, isDead);
    }
    
    private void HandleDamagedEvent()
    {
        _animator.SetTrigger(_damagedTriggerHash);
    }

    private void HandleStunEvent()
    {
        _animator.SetBool(_stunHash, true);
    }
}