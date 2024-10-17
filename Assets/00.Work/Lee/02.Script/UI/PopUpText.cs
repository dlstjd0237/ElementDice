using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine.Serialization;

[Serializable]
public struct TextInfo
{
    public AttackAndTextType textAndTextType;
    [ColorUsage(true, true)] public Color color;
    public float fontSize;
}

public class PopUpText : PoolableMono
{
    [SerializeField] private List<TextInfo> _textInfoList;
    private TextMeshPro _popUpText;

    private Dictionary<AttackAndTextType, TextInfo> _textDictionary;

    protected override void Awake()
    {
        _popUpText = GetComponent<TextMeshPro>();
        _textDictionary = new Dictionary<AttackAndTextType, TextInfo>();
        _textInfoList.ForEach(t=> _textDictionary.Add(t.textAndTextType, t));
    }

   

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
        _popUpText.alpha = 1f;
    }

    private void OnDisable()
    {
        PoolManager.ReturnToPool(gameObject);
    }

    public void ShowText(string text, Vector2 pos, AttackAndTextType andTextType, float yDelta = 1.5f)
    {
        TextInfo textInfo = _textDictionary[andTextType];
        
        _popUpText.text = text;
        _popUpText.transform.position = pos;
        _popUpText.fontSize = textInfo.fontSize;
        _popUpText.color = textInfo.color;
        
        float scaleTime = 0.2f;
        float fadeTime = 1.5f;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(2.5f, scaleTime));
        seq.Append(transform.DOScale(1.2f, scaleTime));
        seq.Append(transform.DOScale(0.3f, fadeTime));
        seq.Join(_popUpText.DOFade(0, fadeTime));
        seq.Join(transform.DOLocalMoveY(pos.y + yDelta, fadeTime));

        seq.OnComplete(() => gameObject.SetActive(false));
    }
}
