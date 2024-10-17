using UnityEngine;
using DG.Tweening;

public class SlashEffect : PoolableMono
{
    [SerializeField]
    private float _fadeDuration;
    [SerializeField]
    private float _sizeDuration;

    private SpriteRenderer _spriteRenderer;

    protected override void Init()
    {
        base.Init();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnDisable()
    {
        transform.localScale = (new Vector3(0, 0f, 0));
        base.OnDisable();
    }

    private void OnEnable()
    {
        Slash();
    }


    public void Slash()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector3(1, 0.2f, 1), _sizeDuration));
        seq.Join(_spriteRenderer.DOFade(1, _sizeDuration));
        seq.AppendInterval(0.05f);
        seq.Append(_spriteRenderer.DOFade(0, 0.2f));
        seq.OnComplete(() =>
        {
            gameObject.SetActive(false);
            seq.Kill();
        });

    }

}
