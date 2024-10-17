using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Define;

public class StartUI : UIToolkit
{
    [SerializeField] private float fadeTimeSet = 0.75f;
    [SerializeField] private string nextSceneName;

    private float _fadeTime;

    private readonly string _startStr = "Label_Start";
    private readonly string _startFadeStr = "Label_StartFade";

    private Label _startLabel;

    private void Awake()
    {
        RootSetting();
        
        _startLabel = Root.Q<Label>(_startStr);

        InputReader.LeftClickEvent += HandleLeftClickEvent;
    }


    private void Update()
    {
        BlinkText();
    }

    private void BlinkText()
    {
        _fadeTime += Time.deltaTime;

        if (fadeTimeSet <= _fadeTime)
        {
            _startLabel.ToggleInClassList(_startFadeStr);
            _fadeTime = 0f;
        }
    }

    private void HandleLeftClickEvent()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        SceneControlManager.FadeOut(() => SceneManager.LoadScene(nextSceneName));
        InputReader.LeftClickEvent -= HandleLeftClickEvent;
    }
}