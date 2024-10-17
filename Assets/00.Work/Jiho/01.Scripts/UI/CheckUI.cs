using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class CheckUI : UIToolkit
{
    private readonly string _checkStr = "VisualElement_Check";
    private readonly string _titleStr = "Label_Title";
    private readonly string _descriptionStr = "Label_Description";
    private readonly string _confirmStr = "Button_Confirm";
    private readonly string _cancelStr = "Button_Cancel";

    private VisualElement _checkVisualElement;
    
    private Label _titleLabel;
    private Label _descriptionLabel;
    
    private Button _confirmButton;
    private Button _cancelButton;

    private void Awake()
    {
        RootSetting();
        
        Root.style.display = DisplayStyle.None;
    }

    private void OnEnable()
    {
        _checkVisualElement = Root.Q<VisualElement>(_checkStr);
        
        _titleLabel = Root.Q<Label>(_titleStr);
        _descriptionLabel = Root.Q<Label>(_descriptionStr);
        
        _confirmButton = Root.Q<Button>(_confirmStr);
        _cancelButton = Root.Q<Button>(_cancelStr);

        _confirmButton.clicked += ConfirmButton;
        _cancelButton.clicked += CancelButton;
    }

    private void ConfirmButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        SceneControlManager.FadeOut(Application.Quit);
    }

    private void CancelButton()
    {
        SoundManager.PlaySound(EAudioType.SFX, EAudioName.Click);
        Root.style.display = DisplayStyle.None;
    }
}
