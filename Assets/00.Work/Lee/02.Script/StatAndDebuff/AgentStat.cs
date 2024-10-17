using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AgentStat : ScriptableObject
{
    [Header("Defensive Stat")]
    public Stat maxHealth;
    public Stat defense;
    public Stat speed;

    [Header("Offensive Stat")]
    public Stat damage;
    
    protected Agent _owner;

    protected Dictionary<StatType, Stat> _statDictionary;

    public virtual void SetOwner(Agent owner)
    {
        _owner = owner;
    }

    public virtual void IncreaseStatBy(int modifyValue, float duration, Stat statToModify)
    {
        _owner.StartCoroutine(StatModifyCoroutine(modifyValue, duration, statToModify));
    }

    private IEnumerator StatModifyCoroutine(int modifyValue, float duration, Stat statToModify)
    {
        statToModify.AddModifier(modifyValue);
        yield return new WaitForSeconds(duration);
        statToModify.RemoveModifier(modifyValue);
    }

    protected virtual void OnEnable()
    {
        _statDictionary = new Dictionary<StatType, Stat>();
    }

    public virtual int GetMeleeDamage()
    {
        return damage.GetValue();
    }

    public int GetApplyDefenseDamage(int currentDamage)
    {
        int calculateDamage = Mathf.Max(1, currentDamage - defense.GetValue());
        return calculateDamage;
    }

    public int GetMaxHealth()
    {
        return maxHealth.GetValue();
    }
}
