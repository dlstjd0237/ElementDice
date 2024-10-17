using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Define;

public class MenuUI : UIToolkit
{
    [SerializeField] private string startSceneName;
    [SerializeField] private string tutorialSceneName;
    
    [SerializeField] private UIDocument settingUI;
    [SerializeField] private UIDocument checkUI;
    
    private readonly string _startStr = "Button_Start";
    private readonly string _tutorialStr = "Button_Tutorial";
    private readonly string _settingStr = "Button_Setting";
    private readonly string _exitStr = "Button_Exit";
    
    private Button _startButton;
    private Button _tutorialButton;
    private Button _settingButton;
    private Button _exitButton;

    private void Awake()
    {
        RootSetting();
    }

    private void OnEnable()
    {
        _startButton = Root.Q<Button>(_startStr);
        _tutorialButton = Root.Q<Button>(_tutorialStr);
        _settingButton = Root.Q<Button>(_settingStr);
        _exitButton = Root.Q<Button>(_exitStr);

        _startButton.clicked += StartButton;
        _tutorialButton.clicked += TutorialButton;
        _settingButton.clicked += SettingButton;
        _exitButton.clicked += ExitButton;
        
        _startButton.RegisterCallback<MouseEnterEvent>(MouseEnterSound);
        _tutorialButton.RegisterCallback<MouseEnterEvent>(MouseEnterSound);
        _settingButton.RegisterCallback<MouseEnterEvent>(MouseEnterSound);
        _exitButton.RegisterCallback<MouseEnterEvent>(MouseEnterSound);
    }

    private void StartButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        StageManager.Instance.ThemaChange(Thema.Jungle);
        SceneControlManager.FadeOut(() => SceneManager.LoadScene(startSceneName));
    }

    private void TutorialButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        SceneControlManager.FadeOut(() => SceneManager.LoadScene(tutorialSceneName));
    }

    private void SettingButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        settingUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void ExitButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        checkUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void MouseEnterSound(MouseEnterEvent enterEvent)
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Hover);
    }
}