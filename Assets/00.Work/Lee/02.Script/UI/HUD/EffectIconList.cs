using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct IconAndBorderDeBuff
{
    public DeBuffEffectType deBuffType;
    public Sprite borderImage;
    public Sprite effectIcon;
}

[Serializable]
public struct IconAndBorderBuff
{
    public BuffEffectType buffType;
    public Sprite borderImage;
    public Sprite effectIcon;
}

public class EffectIconList : MonoBehaviour
{
    [SerializeField] private List<IconAndBorderBuff> _initBuffIcon;
    [SerializeField] private List<IconAndBorderDeBuff> _initDeBuffIcon;

    private Dictionary<BuffEffectType, IconAndBorderBuff> _buffDictionary = new ();
    private Dictionary<DeBuffEffectType, IconAndBorderDeBuff> _deBuffDictionary = new ();
    
    [SerializeField] private EffectIcon _baseIcon;
    [SerializeField] private int _maxLineIconCount;
    [SerializeField] private float _startXPos;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    
    private List<EffectIcon> _iconList = new ();

    private bool _isTweenEnd;

    private void Awake()
    {
        _initBuffIcon.ForEach(x=> _buffDictionary.Add(x.buffType, x));
        _initDeBuffIcon.ForEach(x=> _deBuffDictionary.Add(x.deBuffType, x));
    }

    public void AddIcon<T>(T effectType, int turnCount) where T : System.Enum
    {
        EffectIcon newIcon = Instantiate(_baseIcon, transform);
        if (effectType is DeBuffEffectType deBuff)
        {
            IconAndBorderDeBuff icon = _deBuffDictionary[deBuff];
            newIcon.Init(icon.borderImage, icon.effectIcon, turnCount, deBuff);
        }
        else if(effectType is BuffEffectType buff)
        {
            IconAndBorderBuff icon = _buffDictionary[buff];
            newIcon.Init(icon.borderImage, icon.effectIcon, turnCount, buff);
        }
        _iconList.Add(newIcon);
        SortIcon();
    }
    
    public void ReInitIcon<T>(T effect, int turnCount) where T : System.Enum
    {
        EffectIcon icon = null;
        if (effect is DeBuffEffectType debuff)
            icon = _iconList.Find(x => x.deBuffType == debuff);
        else if(effect is BuffEffectType buff)
            icon = _iconList.Find(x => x.buffType == buff);
        
        if (icon != null) 
            icon.ReInit(turnCount);
        SortIcon();
    }

    public void UsingIcon<T>(T effect, int turnCount, TweenCallback completeAction) where T : System.Enum
    {
        EffectIcon icon = null;
        if (effect is DeBuffEffectType debuff)
            icon = _iconList.Find(x => x.deBuffType == debuff);
        else if(effect is BuffEffectType buff)
            icon = _iconList.Find(x => x.buffType == buff);

        if (icon != null) 
            icon.UseEffect(turnCount, completeAction);
        
        StartCoroutine(CompleteCheck(effect));
    }
    
    public void RemovingIcon<T>(T effect, TweenCallback completeAction) where T : System.Enum
    {
        EffectIcon icon = null;
        if (effect is DeBuffEffectType debuff)
            icon = _iconList.Find(x => x.deBuffType == debuff);
        else if(effect is BuffEffectType buff)
            icon = _iconList.Find(x => x.buffType == buff);
        icon.RemoveEffect(completeAction);

        void RemoveIndex()
        {
            _iconList.Remove(icon);
            SortIcon();
        }
        
        StartCoroutine(CompleteCheck(effect, RemoveIndex));
    }

    private IEnumerator CompleteCheck<T>(T effect, Action completeAction = null) where T : System.Enum
    {
        _isTweenEnd = false;
        EffectIcon icon = null;
        if (effect is DeBuffEffectType debuff)
            icon = _iconList.Find(x => x.deBuffType == debuff);
        else if(effect is BuffEffectType buff)
            icon = _iconList.Find(x => x.buffType == buff);
        
        while (!_isTweenEnd)
        {
            _isTweenEnd = icon.EndCheck();
            yield return null;
        }
        completeAction?.Invoke();
    }

    public bool GetCurrentTweenEnd() => _isTweenEnd;

    private void SortIcon()
    {
        int lineIconCount = 1, lineCount = 1;
        float x = _startXPos, y = 0f;

        foreach (EffectIcon icon in _iconList)
        {
            Vector2 targetPos;
            if (lineIconCount >= _maxLineIconCount)
            {
                lineCount++;
                lineIconCount = 0;
                y += _yOffset;
            }
            
            targetPos.x = x;
            targetPos.y = y;
            icon.transform.DOLocalMove(targetPos, 0.3f);

            if (lineCount % 2 == 0)
                x -= _xOffset;
            else if (lineCount % 2 == 1)
                x += _xOffset;
        }
    }
}
