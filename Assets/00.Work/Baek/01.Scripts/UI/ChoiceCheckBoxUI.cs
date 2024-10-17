using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
public class ChoiceCheckBoxUI : MonoBehaviour
{
    private UIDocument _doc;
    private VisualElement _contain;
    private VisualElement _checkBoxUI;
    private Button _cancelBtn, _checkBtn;
    private void Awake()
    {
        _doc = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _doc.rootVisualElement;
        _contain = root.Q<VisualElement>("contain-box");
        _checkBoxUI = root.Q<VisualElement>("choice_contain-box");
        _cancelBtn = root.Q<Button>("choice_cancel-btn");
        _checkBtn = root.Q<Button>("choice_check-btn");

        _cancelBtn.clicked += () =>
        {
            SetChoiceBoxHide(true);
        };
    }

    public void SetChoiceBoxHide(bool isHide, Action CheckAction = null)
    {


        if (isHide)
        {
            _checkBoxUI.AddToClassList("on");
            _contain.pickingMode = PickingMode.Ignore;

        }
        else
        {
            _checkBoxUI.RemoveFromClassList("on");
            _contain.pickingMode = PickingMode.Position;
            _checkBtn.RegisterCallback<ClickEvent>(evt =>
            {
                CheckAction?.Invoke();
                _checkBtn.UnregisterCallback<ClickEvent>(evt => CheckAction?.Invoke());
                SetChoiceBoxHide(true);
            });
        }
    }
}
