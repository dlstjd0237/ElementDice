using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CreditUp : MonoBehaviour
{
    private void Start()
    {
        SoundManager.PlaySound(EAudioType.BGM, EAudioName.EndingCredits);
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(0,1.5f);
    }
}
