using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static Define;
public class Player : Agent
{
    private Dictionary<Type, IPlayerComponent> _playerComponents;
    private int _diceNum;
    public int DiceNum => _diceNum;

    private Transform _visual;
    private SortingGroup _sortingGroup;

    public PlayerStat PStat => agentStat as PlayerStat;

    public event Action<bool> DeadEvent;

    [SerializeField] public UnityEvent _attackEvent;
    private DiceBezierParent _currentBezierParent;
    public DeBuffEffectType currentDebuff;

    public override void Awake()
    {
        base.Awake();

        BaekPlayerManager.Instance.PlayerSet(this);

        _visual = transform.Find("Visual");
        _sortingGroup = _visual.GetComponent<SortingGroup>();

        _playerComponents = new Dictionary<Type, IPlayerComponent>();

        GetComponentsInChildren<IPlayerComponent>()
            .ToList()
            .ForEach(compo => _playerComponents.Add(compo.GetType(), compo));

        foreach (var compo in _playerComponents.Values)
        {
            compo.Initialize(this);
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        TurnEventBus.Subscribe(TurnEnumType.PlayerAttack, () => _sortingGroup.sortingOrder = 6);
        TurnEventBus.Subscribe(TurnEnumType.EnemyAttack, () => _sortingGroup.sortingOrder = -1);
        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, () =>
        {
            Vector3 pos = Vector3.zero;
            pos.y += 1;
            PopUpText text = PoolManager.SpawnFromPool<PopUpText>("PopUpText", pos);
            text.ShowText("Player Turn", pos , AttackAndTextType.TurnText);
        });

        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, Choice);
        TurnEventBus.Subscribe(TurnEnumType.PlayerDiceRoll, CurrentDiceRoll);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerAttack, () => _sortingGroup.sortingOrder = 6);
        TurnEventBus.UnSubscribe(TurnEnumType.EnemyAttack, () => _sortingGroup.sortingOrder = -1);
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, () =>
        {
            Vector3 pos = Vector3.zero;
            pos.y += 1;
            PopUpText text = PoolManager.SpawnFromPool<PopUpText>("PopUpText",pos);
            text.ShowText("Player Turn", pos, AttackAndTextType.TurnText);
        });
        
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, Choice);
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerDiceRoll, CurrentDiceRoll);
    }

    private void CurrentDiceRoll()
    {
        //_num =  _currentDice.Roll();
        StartCoroutine(EndRollCheckRoutine());
    }

    public void HitSound()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.PlayerHit);
    }

    public void DieSound()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.PlayerDie);
    }

    private IEnumerator EndRollCheckRoutine()
    {
        while (true)
        {
            if (!DiceManager.Instance.IsRoll)
            {
                yield return new WaitForSeconds(0.5f);
                if (_currentBezierParent != null)
                {
                    
                    Vector2 targetPos = _currentBezierParent.isDebuff
                        ? EnemySpawn.Instance.GetEnemy().transform.position
                        : transform.position;
                    targetPos.y += 0.5f;
                    
                    StartCoroutine(_currentBezierParent.StartAttack(targetPos));
                    _currentBezierParent = null;
                }
                else
                    TurnEventBus.Publish(TurnEnumType.PlayerBuffApply);
                
                Debug.Log("버프");
                break;
            }
            yield return null;
        }
    }

    private void Choice()
    {
        Debug.Log("Player : Choice");
    }

    public override void SetDead()
    {
        isDead = true;
        DeadEvent?.Invoke(isDead);
    }

    public override bool SelfCriticalCheck(int damaged) => HealthCompo.CurrentHealth * 0.25f <= damaged;

    public override bool EnemyCriticalCheck(int damaged) =>
        EnemySpawn.Instance.GetEnemy().SelfCriticalCheck(damaged);

    public void Revive()
    {
        isDead = false;
        DeadEvent?.Invoke(isDead);
    }

    public override void Attack(Agent target, AttackAndTextType type)
    {
        if (target.TryGetComponent(out Health health))
        {
            health.ApplyDamage(_diceNum, type, () => _attackEvent?.Invoke());
        }
    }

    public void SetDiceNum(int diceNum) => _diceNum = diceNum;

    public T GetCompo<T>() where T : class
    {
        if (_playerComponents.TryGetValue(typeof(T), out IPlayerComponent compo))
        {
            return compo as T;
        }

        return default;
    }

    public void ReadyAttackOrb(DiceBezierParent currentOrb, bool isDebuff)
    {
        _currentBezierParent = currentOrb;
        _currentBezierParent.isDebuff = isDebuff;
    }
}
