using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class DiceBezierParent : PoolableMono
{
    [SerializeField] private BezierTrail _baseObj;
    [SerializeField] private float _radius;
    [SerializeField] private float _randomRadius;
    [SerializeField] private float _offset;
    private int _trailCount;
    public bool _isCircling;
    private float _delta;

    private List<BezierTrail> _bezierTrails;

    private Action EndedAction;
    public bool isDebuff;

    protected override void OnDisable()
    {
        base.OnDisable();
        if(_bezierTrails == null)
            return;
        
        for (int i = 0; i < _bezierTrails.Count; i++)
        {
            Destroy(_bezierTrails[i].gameObject);
        }
    }

    public void Init(DiceSO diceSo)
    {
        if(diceSo.BuffType == BuffEffectType.None && diceSo.DeBuffType == DeBuffEffectType.None)
            return;

        Color selectColor = Color.white;

        if (diceSo.BuffType != BuffEffectType.None)
            selectColor = ColorManager.Instance.GetBuffColor(diceSo.BuffType);
        else if(diceSo.DeBuffType != DeBuffEffectType.None)
            selectColor = ColorManager.Instance.GetDebuffColor(diceSo.DeBuffType);
        
        _bezierTrails = new List<BezierTrail>();
        _trailCount = 3;
        _isCircling = true;
        for (int i = 0; i < _trailCount; i++)
        {
            BezierTrail obj = Instantiate(_baseObj, transform.position, Quaternion.identity, transform);
            obj.Init(selectColor);
            _bezierTrails.Add(obj);
        }
    }

    public IEnumerator StartAttack(Vector2 targetPos)
    {
        _isCircling = false;
        EndedAction += () => EndedAction = null;
        
        for (int i = 0; i < _bezierTrails.Count; i++)
        {
            Vector2 startPos = _bezierTrails[i].transform.position;
            Vector2 secondPoint = Vector2.Lerp(startPos, targetPos, 0.5f) + Random.insideUnitCircle * _randomRadius;
            if (i == _bezierTrails.Count-1)
                StartCoroutine(_bezierTrails[i].StartBezier(startPos, secondPoint, targetPos,
                    EndedAction, true));
            else
                StartCoroutine(_bezierTrails[i].StartBezier(startPos, secondPoint, 
                    targetPos,null,false));
            yield return DelayTimeManager.Instance.GetDelayTime(0.3f);
        }
    }

    public void AddEndAction(Action addAction) => EndedAction += addAction;
    
    private void Update()
    {
        if (_isCircling)
        {
            _delta += Time.deltaTime;
            for (int i = 0; i < _bezierTrails.Count; i++)
            {
                float x = Mathf.Cos(_delta + i * _offset);
                float y = Mathf.Sin(_delta + i * _offset);
                
                Vector3 pos = new Vector2(x, y) * _radius;
                pos.z = -3f;
                _bezierTrails[i].transform.localPosition = pos;
            }
        }
    }
}
