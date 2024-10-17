using UnityEngine;
using UnityEngine.UI;

public class UI_TabBarBtn : MonoBehaviour
{
    [SerializeField]
    private Button[] btns;


    private void OnEnable()
    {
        for (int i = 0; i < btns.Length; ++i)
        {
            btns[i].onClick.AddListener(HandleBtnEvent);
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < btns.Length; ++i)
        {
            btns[i].onClick.RemoveListener(HandleBtnEvent);
        }
    }

    private void HandleBtnEvent()
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);
    }
}
