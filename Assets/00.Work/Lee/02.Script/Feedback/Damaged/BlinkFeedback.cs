using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[Serializable]
public struct BlinkInfo
{
    public AttackAndTextType attackType;
    [ColorUsage(true, true)] public Color color;
    public bool isOverlay;
    public float blinkTime;
}

public class BlinkFeedback : Feedback
{
    private Transform _visual;
    [SerializeField] private List<BlinkInfo> _blinkList;
    [SerializeField] private Material _blinkMat;
    private Dictionary<AttackAndTextType, BlinkInfo> _blinkInfos;

    [Header("Time")]
    [SerializeField] private float _blinkWaitTime;
    [SerializeField] private float _blinkEndTime;
    
    private SpriteRenderer[] _spriteRenderers;
    private Dictionary<SpriteRenderer, Color> _defaultColors;
    
    private readonly int _colorHash = Shader.PropertyToID("_BlinkColor");
    private readonly int _isOverlayHash = Shader.PropertyToID("_IsOverlay");
    private readonly int _isBlinkHash = Shader.PropertyToID("_IsBlink");
    private readonly int _isOverlayOpacity = Shader.PropertyToID("_OverlayOpacity");

    private Coroutine _coroutine;
    protected override void Awake()
    {
        base.Awake();
        _visual = _agent.transform.Find("Visual");
        
        _defaultColors = new Dictionary<SpriteRenderer, Color>();
        _blinkInfos = new Dictionary<AttackAndTextType, BlinkInfo>();
        _blinkList.ForEach(x=> _blinkInfos.Add(x.attackType, x));

        _spriteRenderers = _visual.GetComponentsInChildren<SpriteRenderer>();
        
        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            if(sprite.color != Color.white)
                _defaultColors[sprite] = sprite.color;
            sprite.material = _blinkMat;
        }
    }

    public override void CreateFeedback()
    {
        _coroutine = StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        Debug.Log(_spriteRenderers);
        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            if(_defaultColors.ContainsKey(sprite))
                sprite.color = _blinkInfos[_agent.currentDamagedType].color;
            
            sprite.material.SetFloat(_isOverlayHash, _blinkInfos[_agent.currentDamagedType].isOverlay ? 1f : 0f);
            sprite.material.SetFloat(_isBlinkHash, 1);
            sprite.material.SetColor(_colorHash, _blinkInfos[_agent.currentDamagedType].color);
            sprite.material.DOFloat(1f, _isOverlayOpacity,  _blinkInfos[_agent.currentDamagedType].blinkTime);
        }

        yield return new WaitForSeconds(_blinkWaitTime);

        foreach (SpriteRenderer sprite in _spriteRenderers)
        {
            if(_defaultColors.TryGetValue(sprite, out Color color))
                sprite.color = color;
            
            sprite.material.DOFloat(0f, _isOverlayOpacity, _blinkEndTime)
                .OnComplete(() => sprite.material.SetFloat(_isBlinkHash, 0));
        }
    }

    public override void FinishFeedback()
    {
        if(_coroutine!=null)
            StopCoroutine(_coroutine);
    }
}
