using System;
using UnityEngine;

public abstract class Feedback : MonoBehaviour
{
    protected Agent _agent;
    
    public abstract void CreateFeedback();
    public abstract void FinishFeedback();

    protected virtual void Awake()
    {
        _agent = GetComponentInParent<Agent>();
    }
    
    protected virtual void OnDisable()
    {
        FinishFeedback();
    }
}
