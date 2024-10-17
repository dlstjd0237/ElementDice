using System;
using System.Collections;
using UnityEngine;

public class PlayerEffect : MonoBehaviour, IPlayerComponent
{
    private Player _player;

    private void OnEnable()
    {
        TurnEventBus.Subscribe(TurnEnumType.PlayerBuffApply, ()=>StartCoroutine(HandleBuffApply()));
        TurnEventBus.Subscribe(TurnEnumType.PlayerBuffRemove, ()=> StartCoroutine(HandleBuffRemove()));
        TurnEventBus.Subscribe(TurnEnumType.PlayerDeBuffApply, ()=> StartCoroutine(_player.EffectiveCompo.ApplyDeBuff()));
        TurnEventBus.Subscribe(TurnEnumType.PlayerDeBuffRemove, ()=> StartCoroutine(_player.EffectiveCompo.RemoveDeBuff()));
    }

    private void OnDisable()
    {
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerBuffApply, ()=>StartCoroutine(HandleBuffApply()));
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerBuffRemove, ()=>StartCoroutine(HandleBuffRemove()));
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerDeBuffApply, ()=> StartCoroutine(_player.EffectiveCompo.ApplyDeBuff()));
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerDeBuffRemove, ()=> StartCoroutine(_player.EffectiveCompo.RemoveDeBuff()));
    }

    public void Initialize(Player player)
    {
        _player = player;
    }
    
    private IEnumerator HandleBuffApply()
    {
        if (_player.EffectiveCompo._currentBuff.Count > 0)
        {
            StartCoroutine(_player.EffectiveCompo.ApplyBuff());
            yield return DelayTimeManager.Instance.GetDelayTime(1f);
        }
        yield return new WaitUntil(() => TurnManager.Instance.IsProcessComplete);
        yield return DelayTimeManager.Instance.GetDelayTime(0.5f);

        TurnEventBus.Publish(TurnEnumType.PlayerAttack);
    }
    
    private IEnumerator HandleBuffRemove()
    {
        if (_player.EffectiveCompo._currentBuff.Count > 0)
        {
            StartCoroutine(_player.EffectiveCompo.RemoveBuff());
            yield return DelayTimeManager.Instance.GetDelayTime(1f);
        }
        yield return new WaitUntil(() => TurnManager.Instance.IsProcessComplete);
        yield return DelayTimeManager.Instance.GetDelayTime(0.5f);
        
        TurnEventBus.Publish(TurnEnumType.PlayerChoice);
    }
}
