using UnityEngine;
using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using static Define;
using System.Reflection;
using UAPT.UI;

#region 사운드 에디터 설정
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(SoundManager))]
public class SoundManagerEditor : Editor
{
    const string INFO = "슬라이더 설정 값\n" +
       "--------------------\n" +
       "Slider Min Value : 0.001\n" +
       "Slider Max Value : 1\n\n" +
        "MixerPath : AudioMixer가 들어있는 폴더 주소\n" +
        "예시) Resources/AudioMixer 폴더 안에있는 \n" +
        "      _Mixer 을 가져오고 싶으면\n" +
        "MixerPath 안에다 AudioMixer/_Mixer 을 써주면 된다.";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
        SoundManager soundManager = (SoundManager)target;
        // [CustomEditor(typeof(SoundManager))] 에 써져있는 target을 가져옴
        if (GUILayout.Button("사운드 데이터 삭제"))
        {
            soundManager.DeleteSoundData();
        }
    }


}
#endif
#endregion
public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private string _mixerPath = "AudioMixer/_Mixer";
    [SerializeField]
    private string _audioContainPath = "AudioSO";
    private SoundContainerSO _so;
    private AudioMixer _mixer;
    private Dictionary<EAudioName, AudioClip> _audioDictionary;
    public Dictionary<EAudioName, AudioClip> AudioDictionary => _audioDictionary;
    private AudioSource _audioSource;


    public AudioMixer Mixer
    {
        get
        {
            MixerNullChake();
            return _mixer;
        }
    }

    private void Awake()
    {
        //_audioSource = gameObject.AddComponent<AudioSource>();
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        _audioSource = GetComponent<AudioSource>();
        FixedScreen.FixedScreenSet();
    }

    private void OnEnable()
    {
        MixerNullChake();
        AudioSONullChake();
    }

    private void AudioSONullChake()
    {
        _so = Resources.Load<SoundContainerSO>(_audioContainPath);
        _audioDictionary = new Dictionary<EAudioName, AudioClip>();
        foreach (EAudioName item in Enum.GetValues(typeof(EAudioName)))
        {
            Type t = typeof(SoundContainerSO);
            FieldInfo info = t.GetField($"_{item.ToString().ToLower()}", BindingFlags.NonPublic | BindingFlags.Instance);

            if (info == null)
            {
                Debug.Log(item);
                continue;
            }

            AudioClip clip = info.GetValue(_so) as AudioClip;

            if (clip == null)
                continue;

            _audioDictionary.Add(item, clip);
        }
    }

    public static void PlaySound(EAudioType type, EAudioName playAudioName, float spawnTime = 4) => Instance._PlaySound(type, playAudioName, spawnTime);

    private void _PlaySound(EAudioType type, EAudioName playAudioName, float spawnTime = 4)
    {
        if (_audioDictionary.ContainsKey(playAudioName) == false)
        {
            Debug.LogError($"{playAudioName} 해당 이넘에 해당하는 Auido Clip이 SO에 존재하지 않습니다.");
            return;
        }

        if (type == EAudioType.SFX)
            PoolManager.SpawnFromPool("SoundObj", Vector3.zero).GetComponent<SoundPlayer>().PlaySound(_audioDictionary[playAudioName], spawnTime);


    }

    //public void ChangeBGM(AudioClip clip)
    //{
    //    _audioSource.clip = clip;
    //    _audioSource.Play();
    //}

    public void DeleteSoundData()
    {
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("SFXVoluem");
        #region 사운드 테스트
#if UNITY_EDITOR

        Debug.Log("사운드 저장 데이터 삭제 됨");
#endif
        #endregion
    }

    public void VolumeSetMaster(float volume)
    {
        MixerNullChake();
        _mixer.SetFloat("Master", Mathf.Log10(volume) * 20); PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void VolumeSetMusic(float volume)
    {
        MixerNullChake();
        _mixer.SetFloat("Music", Mathf.Log10(volume) * 20); PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void VolumeSetSFX(float volume)
    {
        MixerNullChake();
        _mixer.SetFloat("SFX", Mathf.Log10(volume) * 20); PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    /// <summary>
    /// Mixer가 Null인경우 재정의
    /// </summary>
    private void MixerNullChake()
    {
        if (_mixer != null)
            return;
        _mixer = Resources.Load<AudioMixer>(_mixerPath);
    }
}
