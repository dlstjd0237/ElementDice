using UnityEngine;
using static Define;
public class BGMChanger : MonoBehaviour
{
    [SerializeField]
    private EAudioName _playAudioName;
    public EAudioName PlayAudioName
    {
        get => _playAudioName;
        set => _playAudioName = value;
    }
    [SerializeField]
    private SoundContainerSO _so = default;
    private AudioSource _source;
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = SoundManager.Instance.AudioDictionary[_playAudioName];
        _source.Play();
    }

    public void ClipChange(EAudioName eAudioName)
    {
        _playAudioName = eAudioName;
        _source.clip = SoundManager.Instance.AudioDictionary[_playAudioName];
        _source.Play();
    }

}
