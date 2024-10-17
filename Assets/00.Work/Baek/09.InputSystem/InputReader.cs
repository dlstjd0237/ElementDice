using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Console;
[CreateAssetMenu(menuName = "SO/Baek/InputReader")]
public class InputReader : ScriptableObject, IRunTimeActions, IUIActions
{
    private Console _console;
    public Console Console => _console;

    public event Action DiceRollEvent;
    public event Action DiceListViewEvent;
    public event Action EscEvent;
    public event Action LeftClickEvent;


    private void OnEnable()
    {
        if (_console == null)//콘솔이 없을때 새로생성
        {
            _console = new Console();
            _console.RunTime.SetCallbacks(this);
            _console.UI.SetCallbacks(this);
        }
        _console.Enable();
    }

    public void OnDiceListView(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DiceListViewEvent?.Invoke();
        }
    }

    public void OnDiceRoll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DiceRollEvent?.Invoke();
        }
    }

    public void OnESC(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            EscEvent?.Invoke();
        }
    }

    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            LeftClickEvent?.Invoke();
        }
    }
}
