using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ClickToNext : MonoBehaviour
{
    [SerializeField]
    private float startDuration;
    private TextMeshProUGUI _clickToNextText;

    private void Awake()
    {
        _clickToNextText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(SetTweening());
    }

    private IEnumerator SetTweening()
    {
        yield return new WaitForSeconds(startDuration);
        TweeningAlpha();
    }

    private void TweeningAlpha()
    {
        _clickToNextText.DOFade(0.8f, 1f).SetLoops(-1,LoopType.Yoyo);
    }
    
}
