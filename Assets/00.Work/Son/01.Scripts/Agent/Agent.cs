using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using static Define;
//디버프 타입
// public enum DebuffType
// {
//     Burn,
//     Frostbite,
//     Shock,
//     Poisoning
// }

// 추가할 때 무조건 맨 밑에 위에 추가하면 한 칸씩 밀림
public enum AttackAndTextType
{
    NormalAttack,
    CriticalAttack,
    Burn,
    Frost,
    Shock,
    Poison,
    Heal,
    Defense,
    DefaultText,
    Slow,
    TurnText,
}

public abstract class Agent : MonoBehaviour
{
    #region Event

    public UnityEvent OnHitEvent; //맞았을때 이벤트
    public UnityEvent OnDamageEvent; //때릴때 이벤트
    public UnityEvent OnDeadEvent; //죽었을때 이벤트

    #endregion

    #region States
    
    public EAgentType AgentType { get; protected set; }
    
    #endregion

    #region Compo
    //컴포넌트
    public Health HealthCompo { get; protected set; }
    public Animator AnimatorCompo { get; protected set; }
    public HealthBar HealthBarCompo { get; protected set; }
    public AgentEffective EffectiveCompo { get; protected set; }
    public EffectIconList EffectIconListCompo { get; protected set; }
    public AddStatusHUD AddStatusHUDCompo { get; protected set; }
    #endregion

    [SerializeField] protected AgentStat agentStat;
    public AgentStat Stat => agentStat;
    [HideInInspector] public AgentStat defaultStat;

    public bool isDead;
    public bool isStun;
    public bool isWater;

    public AttackAndTextType currentDamagedType = AttackAndTextType.Burn;

    public virtual void OnEnable()
    {
        
    }
    
    public virtual void OnDisable()
    {
        
    }

    public virtual void Awake()
    {
        //초기화
        HealthCompo = GetComponent<Health>();
        HealthCompo.Initialized(this);
        HealthBarCompo = GetComponentInChildren<HealthBar>();

        EffectIconListCompo = GetComponentInChildren<EffectIconList>();
        EffectiveCompo = GetComponent<AgentEffective>();
        EffectiveCompo.Initialize(this);

        AddStatusHUDCompo = GetComponentInChildren<AddStatusHUD>();
        
        AnimatorCompo = transform.Find("Visual").GetComponent<Animator>();

        agentStat = Instantiate(agentStat);
        defaultStat = Instantiate(agentStat);
        Init();
    }

    public virtual void Attack(Agent target, AttackAndTextType type)
    {
        if (target.TryGetComponent(out Health health))
        {
            health.ApplyDamage(Stat.GetMeleeDamage(), type);
        }
    }

    public abstract void SetDead();
    
    protected virtual void Init() { } //초기화 할거 있으면 이곳으로

    public abstract bool SelfCriticalCheck(int damaged);

    public virtual bool EnemyCriticalCheck(int damaged)
    {
        return false;
    }
}