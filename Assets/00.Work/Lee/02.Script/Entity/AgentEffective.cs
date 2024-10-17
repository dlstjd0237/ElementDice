using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentEffective : MonoBehaviour
{
    public List<Debuff> _currentDebuff;
    public List<Buff> _currentBuff;
    
    public bool isBuffHave => _currentBuff.Count > 0;
    public bool isDeBuffHave => _currentDebuff.Count > 0;

    private Agent _agent;
    private EffectIconList _effectIconList;

    public void Initialize(Agent agent)
    {
        _agent = agent;
        _effectIconList = agent.EffectIconListCompo;
        _currentBuff = new List<Buff>();
        _currentDebuff = new List<Debuff>();
    }
    
    public void AddEffect<T>(T effect)
    {
        if(effect is Debuff deBuff)
        {
            if (_currentDebuff.Contains(deBuff))
            {
                _currentDebuff.Find(x => x == deBuff).turnCount = deBuff.turnCount;
                _effectIconList.ReInitIcon(DiceManager.Instance.CurrentDiceSO.DeBuffType, deBuff.turnCount);
                return;
            }

            if (_agent is Enemy enemy)
                _effectIconList.AddIcon(DiceManager.Instance.CurrentDiceSO.DeBuffType, deBuff.turnCount);
            else if (_agent is Player player)
                _effectIconList.AddIcon(player.currentDebuff, deBuff.turnCount);
            
            _currentDebuff.Add(deBuff);
        }
        else if(effect is Buff buff)
        {
            if (_currentBuff.Contains(buff))
            {
                _currentBuff.Find(x => x == buff).turnCount = buff.turnCount;
                _effectIconList.ReInitIcon(DiceManager.Instance.CurrentDiceSO.BuffType, buff.turnCount);
                return;
            }
            
            _effectIconList.AddIcon(DiceManager.Instance.CurrentDiceSO.BuffType, buff.turnCount);
            _currentBuff.Add(buff);
        }
    }
    
    public IEnumerator ApplyDeBuff()
    {
        TurnManager.Instance.SetProcessComplete(false);
        if (_currentDebuff.Count == 0)
            TurnManager.Instance.SetProcessComplete(true);
        
        for (int i = 0; i < _currentDebuff.Count; i++)
        {
            int i1 = i;

            void CompleteAction()
            {
                _currentDebuff[i1].Use(_agent);
                if(_agent.isDead || _agent.isStun)
                    TurnManager.Instance.SetProcessComplete(true);
            }
            _currentDebuff[i].turnCount--;
            _effectIconList.UsingIcon(_currentDebuff[i].deBuffEffectType, _currentDebuff[i].turnCount, CompleteAction);
            yield return new WaitUntil(_effectIconList.GetCurrentTweenEnd);

            if (i == _currentDebuff.Count - 1)
                TurnManager.Instance.SetProcessComplete(true);
            else
                yield return DelayTimeManager.Instance.GetDelayTime(1f);
        }
    }

    public IEnumerator RemoveDeBuff()
    {
        TurnManager.Instance.SetProcessComplete(false);

        if (_currentDebuff.Count == 0)
        {
            TurnManager.Instance.SetProcessComplete(true);
            yield break;
        }

        if (_currentDebuff.FindAll(x=> x.turnCount <= 0).Count == 0)
        {
            TurnManager.Instance.SetProcessComplete(true);
            yield break;
        }

        List<Debuff> removeList = new List<Debuff>();
        
        for (int i = 0; i < _currentDebuff.Count; i++)
        {
            var i1 = i;
            if (_currentDebuff[i].turnCount <= 0)
            {
                removeList.Add(_currentDebuff[i]);
                void CompleteAction()
                {
                    _currentDebuff[i1].RemoveProcess(_agent);
                }
                
                _effectIconList.RemovingIcon(_currentDebuff[i].deBuffEffectType, CompleteAction);
                Debug.Log(_effectIconList.GetCurrentTweenEnd());
                yield return new WaitUntil(_effectIconList.GetCurrentTweenEnd);
                Debug.Log("afafa");
                
                yield return DelayTimeManager.Instance.GetDelayTime(1f);
            }
        }

        foreach (var debuff in removeList)
        {
            _currentDebuff.Remove(debuff);
        }
        
        TurnManager.Instance.SetProcessComplete(true);
    }
    
    public IEnumerator ApplyBuff()
    {
        TurnManager.Instance.SetProcessComplete(false);
        if (_currentBuff.Count == 0)
            TurnManager.Instance.SetProcessComplete(true);
        
        for (int i = 0; i < _currentBuff.Count; i++)
        {
            int i1 = i;
            void CompleteAction()
            {
                _currentBuff[i1].Use(_agent);
                if(_agent.isDead || _agent.isStun)
                    TurnManager.Instance.SetProcessComplete(true);
            }
            _currentBuff[i].turnCount--;
            _effectIconList.UsingIcon(_currentBuff[i].buffEffectType, _currentBuff[i].turnCount, CompleteAction);
            yield return new WaitUntil(_effectIconList.GetCurrentTweenEnd);

            if (i == _currentBuff.Count - 1)
                TurnManager.Instance.SetProcessComplete(true);
            else
                yield return DelayTimeManager.Instance.GetDelayTime(1f);
        }
    }
    
    public IEnumerator RemoveBuff()
    {
        TurnManager.Instance.SetProcessComplete(false);

        if (_currentBuff.Count == 0)
        {
            TurnManager.Instance.SetProcessComplete(true);
            yield break;
        }

        if (_currentBuff.FindAll(x=> x.turnCount <= 0).Count == 0)
        {
            TurnManager.Instance.SetProcessComplete(true);
            yield break;
        }

        List<Buff> removeList = new List<Buff>();
        
        for (int i = 0; i < _currentBuff.Count; i++)
        {
            var i1 = i;
            if (_currentBuff[i].turnCount <= 0)
            {
                removeList.Add(_currentBuff[i]);
                void CompleteAction()
                {
                    _currentBuff[i1].RemoveProcess(_agent);
                }
                
                _effectIconList.RemovingIcon(_currentBuff[i].buffEffectType, CompleteAction);
                Debug.Log(_effectIconList.GetCurrentTweenEnd());
                yield return new WaitUntil(_effectIconList.GetCurrentTweenEnd);
                Debug.Log("afafa");
                
                yield return DelayTimeManager.Instance.GetDelayTime(1f);
            }
        }

        foreach (var debuff in removeList)
            _currentBuff.Remove(debuff);
        TurnManager.Instance.SetProcessComplete(true);
    }
}
