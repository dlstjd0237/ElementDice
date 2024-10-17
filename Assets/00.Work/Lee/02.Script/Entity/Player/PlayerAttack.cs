using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using static Define;

public class PlayerAttack : MonoBehaviour, IPlayerComponent
{
    [SerializeField] private float _moveTime;
    [SerializeField] private float _returnTime;
    [SerializeField] private int _attackRange;
    [SerializeField] private float _attackPosOffset;

    public event Action AttackEvent;
    public event Action<bool> RunEvent;

    private float _attackPosX;
    private Vector3 _defaultPos;
    private Enemy _currentEnemy;

    private Tween _runTween;
    
    private Player _player;

    private void OnEnable()
    {
        TurnEventBus.Subscribe(TurnEnumType.PlayerAttack, AttackStart);
    }
    
    private void OnDisable()
    {
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerAttack, AttackStart);
    }
    
    public void Initialize(Player player)
    {
        _player = player;
        _defaultPos = transform.position;
    }
    
    private void AttackStart()
    {
        _currentEnemy = EnemySpawn.Instance.GetEnemy();
        _attackPosX = _currentEnemy.transform.position.x - _attackPosOffset;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Running(_attackPosX, _moveTime);
        yield return _runTween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        Attack();
        
        yield return new WaitForSeconds(1f);
        
        Running(_defaultPos.x, _returnTime);
        yield return _runTween.WaitForCompletion();
        
        TurnManager.Instance.CurrentTurnEnd();
    }
    
    private void Running(float pos, float time)
    {
        RunEvent?.Invoke(true);
        _runTween = transform.DOMoveX(pos, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            RunEvent?.Invoke(false);
        });
    }

    private void Attack()
    {
        _player.OnDamageEvent?.Invoke();
        AttackEvent?.Invoke();
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Attack);
        
        for (int i = 0; i < _attackRange; i++)
        {
            _currentEnemy = EnemySpawn.Instance.GetEnemy(i);
            if(_currentEnemy is not null)
                _player.Attack(_currentEnemy, AttackAndTextType.NormalAttack);
        }
    }
}
