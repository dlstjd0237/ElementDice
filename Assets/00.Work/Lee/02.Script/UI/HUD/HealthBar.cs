using System;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private Agent _owner;
    [SerializeField] private Image _fillImage;

    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        _owner.OnHitEvent.AddListener(HandleChangeHp);
        _owner.OnDeadEvent.AddListener(()=> HandleActiveHpBar(false) );

        if (_owner as Enemy)
        {
            gameObject.SetActive(false);
            if (_owner == EnemySpawn.Instance.GetEnemy())
                gameObject.SetActive(true);
        }
        _textMeshProUGUI.SetText($"{_owner.HealthCompo.CurrentHealth} / {_owner.Stat.maxHealth.GetValue()}");
    }

    public void HandleActiveHpBar(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void HandleChangeHp()
    {
        float fill = _owner.HealthCompo.GetNormalizeHp();
        _textMeshProUGUI.SetText($"{_owner.HealthCompo.CurrentHealth} / {_owner.Stat.maxHealth.GetValue()}");
        _fillImage.DOFillAmount(fill, _duration);
    }
}
