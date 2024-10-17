using UnityEngine;
using System.Collections;

public class SoundPlayer : PoolableMono
{
    private AudioSource _audio;

    protected override void Init()
    {
        base.Init();
        _audio = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip, float playTime)
    {
        _audio.clip = clip;
        _audio.Play();
        StartCoroutine(SoundTimeCheke(playTime));
    }

    private IEnumerator SoundTimeCheke(float timeOut)
    {
        yield return new WaitForSeconds(timeOut);
        gameObject.SetActive(false);
    }
}
