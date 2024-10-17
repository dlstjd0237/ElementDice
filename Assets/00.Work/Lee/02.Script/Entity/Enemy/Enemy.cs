using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Enemy : Agent
{
    private Dictionary<Type, IEnemyComponent> _enemyComponents;

    public DeBuffEffectType _deBuffEffectType;

    private bool CurrentEnemyCheck => EnemySpawn.Instance.GetEnemy() == this;

    public event Action<bool> DeadEvent;

    public override void Awake()
    {
        base.Awake();
        _enemyComponents = new Dictionary<Type, IEnemyComponent>();

        GetComponentsInChildren<IEnemyComponent>()
            .ToList()
            .ForEach(compo => _enemyComponents.Add(compo.GetType(), compo));

        foreach (var compo in _enemyComponents.Values)
        {
            compo.Initialize(this);
        }
        
        OnHitEvent.AddListener(() => SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.EnemyHit));
    }

    public override void OnEnable()
    {
        base.OnEnable();
        TurnEventBus.Subscribe(TurnEnumType.EnemyDeBuffApply, () =>
        {
            if (!CurrentEnemyCheck)
                return;
            StartCoroutine(EffectiveCompo.ApplyDeBuff());
        });
        TurnEventBus.Subscribe(TurnEnumType.EnemyDeBuffRemove, () =>
        {
            if (!CurrentEnemyCheck)
                return;
            StartCoroutine(EffectiveCompo.RemoveDeBuff());
        });
    }

    public override void OnDisable()
    {
        base.OnDisable();
        TurnEventBus.UnSubscribe(TurnEnumType.EnemyDeBuffApply, () =>
        {
            if (!CurrentEnemyCheck)
                return;
            StartCoroutine(EffectiveCompo.ApplyDeBuff());
        });
        TurnEventBus.UnSubscribe(TurnEnumType.EnemyDeBuffRemove, () =>
        {
            if (!CurrentEnemyCheck)
                return;
            StartCoroutine(EffectiveCompo.RemoveDeBuff());
        });
    }

    public override void SetDead()
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.EnemyDie);
        isDead = true;
        gameObject.SetActive(false);
    }

    public T GetCompo<T>() where T : class
    {
        if (_enemyComponents.TryGetValue(typeof(T), out IEnemyComponent compo))
        {
            return compo as T;
        }

        return default;
    }

    public override bool SelfCriticalCheck(int damaged) => damaged >= 10;
}
