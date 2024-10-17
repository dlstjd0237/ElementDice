using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class BattleBGM : MonoBehaviour
{
    [SerializeField]
    private SoundContainerSO _so = default;
    private AudioSource _source;
    private void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.clip = SoundManager.Instance.AudioDictionary[GetaudioNameToStageName()];
        _source.Play();
    }



    public EAudioName GetaudioNameToStageName()
    {
        Thema thema = StageManager.Instance.currentThema;

        if (Enum.TryParse<EAudioName>(thema.ToString(), out EAudioName result))
        {
            return result;
        }

        return EAudioName.Addiction;  
    }
}
