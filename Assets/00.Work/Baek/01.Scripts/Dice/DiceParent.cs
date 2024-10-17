using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DiceParent : MonoBehaviour
{
    private List<Dice> _diceList;
    private void Awake()
    {
        _diceList = transform.GetComponentsInChildren<Dice>().ToList();
        DiceManager.Instance.SetDiceList(_diceList);
    }

    private void Start()
    {
        DiceManager.Instance.DiceInit();
    }

    private void OnEnable()
    {
        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, HandleRoll);
    }

    private void HandleRoll()
    {
        DiceManager.Instance.IsAttacking = false;
    }

    private void OnDisable()
    {
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, HandleRoll);
    }
}
