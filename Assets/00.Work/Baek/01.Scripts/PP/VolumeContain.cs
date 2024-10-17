using System;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using UnityEngine.Rendering.Universal;

public class VolumeContain : MonoBehaviour
{
    #region ValueDefine

    private const float CA_START_DURATION = 0.2f;
    private const float CA_END_DURATION = 0.4f;

    #endregion



    public event Action VolumeChanged;

    [SerializeField]
    private VolumeProfile _profile;

    public VolumeProfile Profile
    {
        get => _profile;
        set
        {
            _profile = value;
            VolumeChanged?.Invoke();
        }
    }

    private ChromaticAberration _ca;
    private LensDistortion _ld;


    private void Awake()
    {
        if (Profile.TryGet(out ChromaticAberration ca))
            _ca = ca;

        if (Profile.TryGet(out LensDistortion ld))
            _ld = ld;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    SetChromaticAberration(0.7f);
        //    SetLensDistortion(-0.5f);
        //}


    }


    public void AttackEvent()
    {
        SetChromaticAberration(0.7f);
        SetLensDistortion(-0.5f);
    }
    public void SetChromaticAberration(float vlaue)
    {
        var intensity = _ca.intensity;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => intensity.value, x => intensity.value = x, vlaue, CA_START_DURATION * 0.5f).SetEase(Ease.InExpo));
        seq.Append(DOTween.To(() => intensity.value, x => intensity.value = x, 0, CA_END_DURATION).SetEase(Ease.Linear))
            .OnComplete(() => seq.Kill());
    }

    public void SetLensDistortion(float vlaue)
    {
        var intensity = _ld.intensity;
        var scale = _ld.scale;
        Sequence seq = DOTween.Sequence();
        seq.Append(DOTween.To(() => intensity.value, x => intensity.value = x, vlaue, CA_START_DURATION * 0.5f).SetEase(Ease.InExpo));
        seq.Join(DOTween.To(() => scale.value, x => scale.value = x, 0.9f, CA_START_DURATION * 0.5f).SetEase(Ease.InExpo));
        seq.Append(DOTween.To(() => intensity.value, x => intensity.value = x, 0, CA_END_DURATION).SetEase(Ease.Linear));
        seq.Join(DOTween.To(() => scale.value, x => scale.value = x, 1, CA_END_DURATION).SetEase(Ease.Linear))
            .OnComplete(() => seq.Kill());
    }

}
