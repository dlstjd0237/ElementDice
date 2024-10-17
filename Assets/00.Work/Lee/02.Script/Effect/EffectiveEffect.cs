using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectiveEffect : PoolableMono
{
    private ParticleSystem _prSystem;
    public bool isPlay => _prSystem.isPlaying;
    protected override void Awake()
    {
        base.Awake();
        _prSystem = GetComponent<ParticleSystem>();
    }

    public void Init(Color color)
    {
        gameObject.SetActive(false);
        _prSystem.startColor = color;
    }

    public void Play()
    {
        gameObject.SetActive(true);
        _prSystem.Play();
    }
}
