using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using static Define;

public class EnemyAttack : MonoBehaviour, IEnemyComponent
{
    [SerializeField] private float _moveTime;
    [SerializeField] private float _returnTime;
    [SerializeField] private float _attackPosOffset;
    [SerializeField] private int _effectRandom;

    public event Action AttackEvent;
    public event Action<bool> RunEvent;

    private float _attackPosX;
    private Vector3 _defaultPos;
    private Player _player;
    private Enemy _enemy;

    private Tween _runTween;

    private bool isDebuffAttack;

    private void OnEnable()
    {
        TurnEventBus.Subscribe(TurnEnumType.EnemyAttack, ()=>
        {
            if(_enemy != EnemySpawn.Instance.GetEnemy())
                return;
            AttackStart();
        });
    }
    
    private void OnDisable()
    {
        TurnEventBus.UnSubscribe(TurnEnumType.EnemyAttack,()=>    
        {
            if(_enemy != EnemySpawn.Instance.GetEnemy())
                return;
            AttackStart();
        });
    }
    
    public void Initialize(Enemy enemy)
    {
        _enemy = enemy;
        _defaultPos = transform.localPosition;
    }
    
    private void AttackStart()
    {
        _player = BaekPlayerManager.Instance.Player;
        _defaultPos = transform.localPosition;
        _attackPosX = _player.transform.position.x - _attackPosOffset;
        StartCoroutine(EffectAttack());
    }

    private IEnumerator EffectAttack()
    {
        if (_enemy._deBuffEffectType == DeBuffEffectType.None)
        {
            StartCoroutine(AttackRoutine());
            yield break;
        }
        
        int rand = Random.Range(1, 101);
        if (rand <= _effectRandom)
        {
            isDebuffAttack = true;
            
            Vector3 pos = transform.position;
            pos.y += 0.5f;
            
            EffectiveEffect effect = PoolManager.SpawnFromPool<EffectiveEffect>("EnemyEffect", pos);
            Color color = Color.white;
            color = ColorManager.Instance.GetDebuffColor(_enemy._deBuffEffectType);
            effect.Init(color);
            effect.Play();

            _player.currentDebuff = _enemy._deBuffEffectType;
            AttackEvent += () => DiceManager.Instance.GetDebuff(_enemy._deBuffEffectType).Adding(_player);
            
            while (effect.isPlay)
            {
                yield return null;
            }
        }
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Running(_attackPosX, _moveTime);
        yield return _runTween.WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        Attack();
        
        yield return new WaitForSeconds(0.5f);
        
        Running(_defaultPos.x, _returnTime);
        yield return _runTween.WaitForCompletion();
        
        TurnManager.Instance.CurrentTurnEnd();
    }
    
    private void Running(float pos, float time)
    {
        RunEvent?.Invoke(true);
        _runTween = transform.DOLocalMoveX(pos, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            RunEvent?.Invoke(false);
        });
    }

    private void Attack()
    {
        _enemy.OnDamageEvent?.Invoke();
        AttackEvent?.Invoke();
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Attack);

        if (isDebuffAttack)
        {
            isDebuffAttack = false;
            AttackEvent -= () => DiceManager.Instance.GetDebuff(_enemy._deBuffEffectType).Adding(_player);
        }
        
        if(_player is not null)
            _enemy.Attack(_player, AttackAndTextType.NormalAttack);
    }
}
