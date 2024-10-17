using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class BattleManager : MonoSingleton<BattleManager>
{
    [SerializeField] private UnityEvent _clearEvent;
    [SerializeField] private UnityEvent _failEvent;
    public UnityEvent ClearEvent => _clearEvent;
    public UnityEvent FailEvent => _failEvent;

    private Player _player;

    public bool EnemyStun => EnemySpawn.Instance.GetEnemy().isStun;
    public bool EnemyDead => EnemySpawn.Instance.GetEnemy().isDead;
    public bool EnemyWater => EnemySpawn.Instance.GetEnemy().isWater;

    public bool PlayerStun => _player.isStun;
    public bool PlayerDead => _player.isDead;
    public bool PlayerWater => _player.isWater;

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _player = BaekPlayerManager.Instance.GetPlayer();
    }

    private void OnEnable()
    {
        TurnEventBus.Subscribe(TurnEnumType.PlayerTurnEnd, EnemyDeadProcess);
        TurnEventBus.Subscribe(TurnEnumType.PlayerTurnEnd, PlayerDeadCheck);

        TurnEventBus.Subscribe(TurnEnumType.EnemyTurnEnd, PlayerDeadCheck);
        TurnEventBus.Subscribe(TurnEnumType.EnemyTurnEnd, EnemyTurnChange);

        TurnEventBus.Subscribe(TurnEnumType.BattleClear, DebugBattleClear);
        TurnEventBus.Subscribe(TurnEnumType.BattleFail, DebugBattleFail);
    }

    private void OnDisable()
    {
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerTurnEnd, EnemyDeadProcess);
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerTurnEnd, PlayerDeadCheck);

        TurnEventBus.UnSubscribe(TurnEnumType.EnemyTurnEnd, PlayerDeadCheck);
        TurnEventBus.UnSubscribe(TurnEnumType.EnemyTurnEnd, EnemyTurnChange);

        TurnEventBus.UnSubscribe(TurnEnumType.BattleClear, DebugBattleClear);
        TurnEventBus.UnSubscribe(TurnEnumType.BattleFail, DebugBattleFail);
    }

    private void DebugBattleClear()
    {
        _clearEvent?.Invoke();
    }
    
    private void DebugBattleFail()
    {
        Debug.Log("SDfadfdfasdf");
    }

    private void PlayerDeadCheck()
    {
        if (_player.isDead)
        {
            TurnManager.Instance.BattleEndSetting(true);
            TurnEventBus.Publish(TurnEnumType.BattleFail);
        }
    }

    private void EnemyTurnChange()
    {
        if (EnemyDead)
            EnemyDeadProcess();
        else
            TurnManager.Instance.TurnSetting(true);
    }

    private void EnemyDeadProcess()
    {
        if (EnemyDead)
        {
            EnemySpawn.Instance.CheckSortingEnemy();
            if (EnemySpawn.Instance.GetEnemy() == null)
            {
                //웨이브 클리어(아윤 선배에게 물어보기) 나는 지호지호몬
                TurnManager.Instance.BattleEndSetting(true);
                TurnEventBus.Publish(TurnEnumType.BattleClear);

                if (StageManager.Instance.GetPage() == EnemyPage.PageBoss)
                {
                    if (StageManager.Instance.currentThema == Thema.Mountain)
                    {
                        SceneControlManager.FadeOut(() => SceneManager.LoadScene("Ending"));
                        return;
                    }
                    
                    StageManager.Instance.ThemaChange(StageManager.Instance.currentThema + 1);
                }
                return;
            }
            TurnManager.Instance.TurnSetting(FirstTurnSetting());
        }
        else
        {
            TurnManager.Instance.TurnSetting(false);
        }
    }

    public void EnemyStunEnd()
    {
        if (EnemyStun)
            EnemySpawn.Instance.GetEnemy().isStun = false;
    }

    public void PlayerStunEnd()
    {
        if (PlayerStun)
            _player.isStun = false;
    }
    
    public void EnemyWaterEnd()
    {
        if (EnemyWater)
        {
            Enemy enemy = EnemySpawn.Instance.GetEnemy();
            enemy.Stat.damage.SetDefaultValue(enemy.defaultStat.damage.GetValue());
            enemy.isWater = false;
        }
    }
    
    public void PlayerWaterEnd()
    {
        if (PlayerWater)
        {
            _player.Stat.damage.SetDefaultValue(_player.defaultStat.damage.GetValue());
            _player.isWater = false;
        }
    }
    public bool FirstTurnSetting()
    {
        return _player.PStat.speed.GetValue() >= EnemySpawn.Instance.GetEnemy().Stat.speed.GetValue();
    }
}
