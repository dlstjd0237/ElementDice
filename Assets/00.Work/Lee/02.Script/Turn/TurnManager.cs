using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TurnManager : MonoSingleton<TurnManager>
{
    public bool IsPlayerTurn { get; set; } = true;
    private bool _battleEnd;

    private bool _isProcessComplete;
    public bool IsProcessComplete =>_isProcessComplete;

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public void BattleStart()
    {
        BattleEndSetting(false);
        TurnSetting(BattleManager.Instance.FirstTurnSetting());
        CurrentTurnStart();
    }

    public void CurrentTurnStart() => StartCoroutine(CurrentTurnStartRoutine());

    private IEnumerator CurrentTurnStartRoutine()
    {
        if (_battleEnd)
            yield break;
        
        if (!IsPlayerTurn)
            yield return EnemyProcess();
        else
            yield return PlayerProcess();
    }
    
    // 턴 흐름 EnemyDeBuffRemove -> EnemyDeBuffApply ->
    // 만약 DeBuff 받으면서 죽거나 스턴되면 -> EnemyTurnEnd -> Player 턴으로 
    // 아니면 -> EnemyAttack -> EnemyTurnEnd -> Player 턴으로 
    private IEnumerator EnemyProcess()
    {
        Vector3 pos = Vector3.zero;
        pos.y += 1;
        PopUpText text = PoolManager.SpawnFromPool<PopUpText>("PopUpText",pos);
        text.ShowText("Enemy Turn", pos, AttackAndTextType.TurnText);
        
        Enemy enemy = EnemySpawn.Instance.GetEnemy();
        
        TurnEventBus.Publish(TurnEnumType.EnemyDeBuffRemove);
        yield return DelayTimeManager.Instance.GetDelayTime(0.5f);
        yield return new WaitUntil(() => _isProcessComplete);
        
        TurnEventBus.Publish(TurnEnumType.EnemyDeBuffApply);
        yield return new WaitUntil(() => _isProcessComplete);
        
        yield return DelayTimeManager.Instance.IsDelayCheck(enemy.EffectiveCompo.isDeBuffHave, 1f, 0.5f);
        if (BattleManager.Instance.EnemyDead || BattleManager.Instance.EnemyStun)
        {
            Debug.Log("fafsfasd");
            CurrentTurnEnd();
        }
        else
            TurnEventBus.Publish(TurnEnumType.EnemyAttack);
    } 
    
    // 턴 흐름 PlayerDeBuffRemove -> PlayerDeBuffApply ->
    // 만약 DeBuff 받으면서 죽거나 스턴되면 -> PlayerTurnEnd -> Enemy 턴으로 
    // 아니면 PlayerBuffRemove -> PlayerChoice -> PlayerDiceRoll
    // -> PlayerBuffApply -> PlayerAttack -> PlayerTurnEnd -> Enemy 턴으로 
    private IEnumerator PlayerProcess()
    {
        Player player = BaekPlayerManager.Instance.Player;
        
        TurnEventBus.Publish(TurnEnumType.PlayerDeBuffRemove);
        yield return DelayTimeManager.Instance.GetDelayTime(0.5f);
        yield return new WaitUntil(() => _isProcessComplete);
        
        TurnEventBus.Publish(TurnEnumType.PlayerDeBuffApply);
        yield return new WaitUntil(() => _isProcessComplete);

        yield return DelayTimeManager.Instance.IsDelayCheck(player.EffectiveCompo.isDeBuffHave,
            1.5f, 0);
        if (BattleManager.Instance.PlayerStun || BattleManager.Instance.PlayerDead)
            CurrentTurnEnd();
        else
            TurnEventBus.Publish(TurnEnumType.PlayerBuffRemove);
    }

    public void CurrentTurnEnd()
    {
        if (!IsPlayerTurn)
        {
            BattleManager.Instance.EnemyStunEnd();
            BattleManager.Instance.EnemyWaterEnd();
            TurnEventBus.Publish(TurnEnumType.EnemyTurnEnd);
            
        }
        else
        {
            BattleManager.Instance.PlayerStunEnd();
            BattleManager.Instance.PlayerWaterEnd();
            TurnEventBus.Publish(TurnEnumType.PlayerTurnEnd);
        }
        
        CurrentTurnStart();
    }
    public void TurnSetting(bool isPlayerTurn) => IsPlayerTurn = isPlayerTurn;

    public void BattleEndSetting(bool isActive) => _battleEnd = isActive;
    
    public void DiceRoll() => TurnEventBus.Publish(TurnEnumType.PlayerDiceRoll);
    public void SetProcessComplete(bool isActive) => _isProcessComplete = isActive;
}
