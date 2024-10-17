using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EffectIcon : MonoBehaviour
{
    private Image _borderImage;
    private Image _effectIcon;
    private TextMeshProUGUI _turnCountText;
    private CanvasGroup _canvasGroup;

    private Vector3 _defaultScale;
    
    private bool _isEnd;
    
    public DeBuffEffectType deBuffType;
    public BuffEffectType buffType;
    
    [SerializeField] private float _scale;
    [SerializeField] private float _upYvalue;
    [SerializeField] private float _shakePower;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _borderImage = transform.Find("BorderImage").GetComponent<Image>();
        _effectIcon = transform.Find("EffectImage").GetComponent<Image>();
        _turnCountText = transform.Find("TurnCount").GetComponent<TextMeshProUGUI>();

        _defaultScale = transform.localScale;
    }

    public void Init<T>(Sprite borderImage, Sprite effectIcon, int turnCount, T effectType)
    {
        if (effectType is DeBuffEffectType deBuff)
            deBuffType = deBuff;
        else if (effectType is BuffEffectType buff)
            buffType = buff;
        
        _borderImage.sprite = borderImage;
        _effectIcon.sprite = effectIcon;
        _turnCountText.SetText(turnCount.ToString());
    }

    public void ReInit(int turnCount) => _turnCountText.SetText(turnCount.ToString());

    public void UseEffect(int turnCount, TweenCallback completeAction)
    {
        _isEnd = false;
        Sequence seq = DOTween.Sequence();
        seq.OnComplete(() =>
        {
            _turnCountText.SetText(turnCount.ToString());
            _isEnd = true;
        });

        // 현재 이펙트 아이콘 위로 올라가고 커짐
        seq.Append(transform.DOScale(Vector3.one * _scale, 0.3f));
        seq.Join(transform.DOLocalMoveY(transform.position.y + _upYvalue, 0.3f));
        seq.AppendInterval(1f);
        
        // 흔들리면서 액션 실행
        seq.Append(transform.DOShakePosition(0.3f, _shakePower, 20));
        seq.JoinCallback(completeAction);

        seq.AppendInterval(0.5f);

        // 다시 내려오면서 작아짐
        seq.Append(transform.DOScale(_defaultScale, 0.15f));
        seq.Join(transform.DOLocalMoveY(transform.position.y - _upYvalue, 0.15f));
    }
    
    public void RemoveEffect(TweenCallback completeAction)
    {
        _isEnd = false;
        Sequence seq = DOTween.Sequence();
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            _isEnd = true;
        });

        // 현재 이펙트 아이콘 위로 올라가고 커짐
        seq.Append(transform.DOScale(Vector3.one * _scale, 0.3f));
        seq.Join(transform.DOLocalMoveY(transform.position.y + _upYvalue, 0.3f));
        seq.AppendInterval(1f);
        
        // 흔들리면서 액션 실행
        seq.Append(_canvasGroup.DOFade(0, 0.25f));
        seq.JoinCallback(completeAction);
    }

    public bool EndCheck() => _isEnd;
}
