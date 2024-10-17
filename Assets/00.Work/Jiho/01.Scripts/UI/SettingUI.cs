using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class SettingUI : UIToolkit
{
    private readonly string _masterStr = "SliderInt_Master";
    private readonly string _bgmStr = "SliderInt_BGM";
    private readonly string _sfxStr = "SliderInt_SFX";
    private readonly string _screenStr = "DropdownField_Screen";
    private readonly string _closeStr = "Button_Close";

    private SliderInt _masterSliderInt;
    private SliderInt _bgmSliderInt;
    private SliderInt _sfxSliderInt;

    private DropdownField _screenDropdownField;

    private Button _closeButton;

    private void Awake()
    {
        RootSetting();

        _masterSliderInt = Root.Q<SliderInt>(_masterStr);
        _bgmSliderInt = Root.Q<SliderInt>(_bgmStr);
        _sfxSliderInt = Root.Q<SliderInt>(_sfxStr);

        _screenDropdownField = Root.Q<DropdownField>(_screenStr);

        _closeButton = Root.Q<Button>(_closeStr);
        
        float masterValue = PlayerPrefs.GetFloat("MasterVolume", 0.5f) * 100; // 0.5f : 디폴트 값 (전 값 없을 때)
        _masterSliderInt.value = (int)masterValue;

        float bgmValue = PlayerPrefs.GetFloat("MusicVolume", 0.5f) * 100;
        _bgmSliderInt.value = (int)bgmValue;

        float sfxValue = PlayerPrefs.GetFloat("SFXVolume", 0.5f) * 100;
        _sfxSliderInt.value = (int)sfxValue;

        Debug.Log($"masterValue : {masterValue}");

        SoundManager.Instance.VolumeSetMaster(masterValue / 100);
        SoundManager.Instance.VolumeSetMusic(bgmValue / 100);
        SoundManager.Instance.VolumeSetSFX(sfxValue / 100);

        _masterSliderInt.RegisterValueChangedCallback(MasterSlider);
        _bgmSliderInt.RegisterValueChangedCallback(BGMSlider);
        _sfxSliderInt.RegisterValueChangedCallback(SFXSlider);
        
        // 전체 화면 : 디폴트 값 (전 값 없을 때)
        _screenDropdownField.choices = new List<string> { "전체 화면", "창 화면" };
        
        string screenValue = PlayerPrefs.GetString("ScreenSetting", "전체 화면");
        
        ScreenSetting(screenValue);
        
        if (_screenDropdownField.choices.Contains(screenValue))
            _screenDropdownField.value = screenValue;
        else
            Debug.LogError("screenValue가 choices에 없음.");

        _screenDropdownField.RegisterValueChangedCallback(ScreenDropdown);
        
        InputReader.EscEvent += InGameEscapeButton;
        _closeButton.clicked += CloseButton;

        _closeButton.RegisterCallback<MouseEnterEvent>(delegate
        {
            SoundManager.PlaySound(EAudioType.SFX, EAudioName.Hover);
        });
    }

    
    

    private void Start()
    {
        Root.style.display = DisplayStyle.None;
    }

    private void MasterSlider(ChangeEvent<int> changeEvent)
    {
        SoundManager.Instance.VolumeSetMaster((float)changeEvent.newValue / 100);
        Debug.Log($"전체음 : {changeEvent.newValue}");
    }

    private void BGMSlider(ChangeEvent<int> changeEvent)
    {
        SoundManager.Instance.VolumeSetMusic((float)changeEvent.newValue / 100);
        Debug.Log($"배경음 : {changeEvent.newValue}");
    }

    private void SFXSlider(ChangeEvent<int> changeEvent)
    {
        SoundManager.Instance.VolumeSetSFX((float)changeEvent.newValue / 100);
        Debug.Log($"효과음 : {changeEvent.newValue}");
    }

    private void ScreenDropdown(ChangeEvent<string> changeEvent)
    {
        Debug.Log($"화면 설정 : {changeEvent.newValue}");
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        ScreenSetting(changeEvent.newValue);
    }

    private void ScreenSetting(string newValue)
    {
        switch (newValue)
        {
            case "전체 화면":
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                PlayerPrefs.SetString("ScreenSetting", newValue);
                break;

            case "창 화면":
                Screen.fullScreenMode = FullScreenMode.Windowed;
                PlayerPrefs.SetString("ScreenSetting", newValue);
                break;
        }
    }

    private void CloseButton()
    {
        Root.style.display = DisplayStyle.None;

        InputReader.Console.UI.DiceListView.Enable();
        InputReader.Console.RunTime.DiceRoll.Enable();

        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
    }

    public void InGameEscapeButton()
    {
        if (Root.style.display == DisplayStyle.None)
        {
            Root.style.display = DisplayStyle.Flex;

            InputReader.Console.UI.DiceListView.Disable();
            InputReader.Console.RunTime.DiceRoll.Disable();
        }
        else
        {
            Root.style.display = DisplayStyle.None;

            InputReader.Console.UI.DiceListView.Enable();
            InputReader.Console.RunTime.DiceRoll.Enable();
        }
    }
}