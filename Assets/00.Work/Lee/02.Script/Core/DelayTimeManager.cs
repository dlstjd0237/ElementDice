using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTimeManager : MonoSingleton<DelayTimeManager>
{
    private Dictionary<float, WaitForSeconds> _delayTimes = new ();

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
    }

    public IEnumerator IsDelayCheck(bool isDelay, float trueTime, float falseTime)
    {
        if (isDelay)
        { 
            Debug.Log("true"); 
            yield return GetDelayTime(trueTime);
        }
        else
        {
            Debug.Log("false");
            yield return GetDelayTime(falseTime);
        }
    }
    
    public WaitForSeconds GetDelayTime(float time)
    {
        if (!_delayTimes.ContainsKey(time))
            _delayTimes.Add(time, new WaitForSeconds(time));
        
        return _delayTimes[time];
    }
}
