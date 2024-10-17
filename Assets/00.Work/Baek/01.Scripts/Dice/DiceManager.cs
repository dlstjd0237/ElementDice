using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UAPT.Utile;
using Random = UnityEngine.Random;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Define;

public class DiceManager : MonoSingleton<DiceManager>
{
    [SerializeField] private InputReader _input;
    [SerializeField] private DiceColorListSO _diceColorListSO;
    [SerializeField] private List<Dice> _diceList;
    [SerializeField] private TextMeshProUGUI _diceNumTxt;

    private Dictionary<DiceRareType, Color> _diceColorDictionary;
    private Dictionary<DiceSidedType, Dice> _diceContainDictionary;
    private Dictionary<DeBuffEffectType, Debuff> _debuffDictionary;
    private Dictionary<BuffEffectType, Buff> _buffDictionary;

    [SerializeField] private DiceSO _currentDiceSO = default;
    public DiceSO CurrentDiceSO => _currentDiceSO;

    [SerializeField] private DiceSO _debugDice;

    private DiceContinViewUI _diceContainViewUI;
    public DiceContinViewUI DiceContainViewUI => _diceContainViewUI;

    public Dictionary<string, DiceSO> DiceCardDictionary => _diceCardDictionary;
    private Dictionary<string, DiceSO> _diceCardDictionary;

    public Dictionary<string, int> DiceAmountDictionary => _diceAmountDictionary;
    private Dictionary<string, int> _diceAmountDictionary;

    public event Action<DiceSO> DiceAddEvent;
    public event Action<DiceSO> ChangeDiceEvent;

    private bool _isRoll = false;
    private bool _canDiceChange = true;
    private bool _canDiceRoll = false;

    private DiceBezierParent _diceOrb = null;

    public bool IsRoll
    {
        get => _isRoll;
        set => _isRoll = value;
    }

    private bool _isAttacking = false;

    public bool IsAttacking
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        _diceCardDictionary = new Dictionary<string, DiceSO>();
        _diceCardDictionary.Add(_currentDiceSO.DiceName, _currentDiceSO);

        _diceAmountDictionary = new Dictionary<string, int>();
        _diceAmountDictionary.Add(_currentDiceSO.DiceName, 0);

        Debug.Log("다이스 메니져");
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))
            AddDiceCardDictionary((_debugDice));
#endif
    }


    private void OnEnable()
    {
        ColorDictionaryInit();
        DebuffDictionaryInit();
        _input.DiceRollEvent += HandleDiceRoll;
        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, () => HandleDiceChangeOrbInit(_currentDiceSO));
        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, () => _canDiceChange = true);
        TurnEventBus.Subscribe(TurnEnumType.PlayerChoice, () => _canDiceRoll = true);
        ChangeDiceEvent += HandleDiceChangeOrbInit;
    }

    private void OnDisable()
    {
        _input.DiceRollEvent -= HandleDiceRoll;
        ChangeDiceEvent -= HandleDiceChangeOrbInit;
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, () => HandleDiceChangeOrbInit(_currentDiceSO));
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, () => _canDiceChange = true);
        TurnEventBus.UnSubscribe(TurnEnumType.PlayerChoice, () => _canDiceRoll = true);
    }

    private void Start()
    {
        DiceInit();
    }

    public void AddDiceCardDictionary(DiceSO SO)
    {
        string DiceName = SO.DiceName;
        if (_diceCardDictionary.ContainsKey(DiceName))
        {
            _diceAmountDictionary[SO.DiceName]++;
        }
        else
        {
            _diceCardDictionary.Add(SO.DiceName, SO);
            _diceAmountDictionary.Add(SO.DiceName, 0);
            DiceAddEvent?.Invoke(SO);
        }
    }

    public void DiceNumTextActive(bool vlaue)
    {
        if(_diceNumTxt != null)
            _diceNumTxt.enabled = vlaue;
        if(_diceOrb != null)
            _diceOrb.gameObject.SetActive(vlaue);
    }

    public void DiceInit()
    {
        _diceNumTxt.enabled = false;
        if (_diceContainDictionary == null)
            return;

        _diceNumTxt.enabled = true;
        _diceContainDictionary[_currentDiceSO.DiceSidedType].gameObject.SetActive(true);
        ChangeDiceEvent?.Invoke(_currentDiceSO);
    }

    public void DiceAnimationEnd()
    {
        _diceNumTxt.enabled = true;
    }

    public void SetDiceList(List<Dice> diceList)
    {
        _diceList = diceList;
        DiceDictionaryInit();
    }

    private void DiceDictionaryInit() //_diceContainDictionary 초기화
    {
        _diceContainDictionary = new Dictionary<DiceSidedType, Dice>();
        for (int i = 0; i < _diceList.Count; ++i)
        {
            _diceContainDictionary.Add(_diceList[i].DiceSidedType, _diceList[i]);
            _diceContainDictionary[_diceList[i].DiceSidedType].gameObject.SetActive(false);
        }
    }

    private void HandleDiceRoll()
    {
        if (_isAttacking == true || !_canDiceRoll)
            return;

        int DiceSide = (int)_currentDiceSO.DiceSidedType;

        int randNum = Random.Range(1, DiceSide + 1);
        _diceNumTxt.SetText(randNum.ToString());
        _diceNumTxt.enabled = false;
        _isRoll = true;
        _canDiceChange = false;
        _canDiceRoll = false;
        _isAttacking = true;
        List<bool> list = _currentDiceSO.GetEffectNum();

        Player player = BaekPlayerManager.Instance.GetPlayer();
        player.SetDiceNum(randNum);

        EAudioName eAudioName = GetAudioName(_currentDiceSO.DeBuffType);
        if (eAudioName == EAudioName.None)
            eAudioName = GetAudioName(_currentDiceSO.BuffType);


        SoundManager.PlaySound(EAudioType.SFX, eAudioName);

        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i] == true && i + 1 == randNum)
            {
                if (_currentDiceSO.DeBuffType != DeBuffEffectType.None)
                {
                    if (_diceOrb != null)
                    {
                        _diceOrb.AddEndAction(() =>
                            GetDebuff(_currentDiceSO.DeBuffType).Adding(EnemySpawn.Instance.GetEnemy()));
                        player.ReadyAttackOrb(_diceOrb, true);
                    }
                }

                if (_currentDiceSO.BuffType != BuffEffectType.None)
                {
                    if (_diceOrb != null)
                    {
                        _diceOrb.AddEndAction(() =>
                            GetBuff(_currentDiceSO.BuffType).Adding(player)); //<- 여기에 Player가 들어가야함
                        player.ReadyAttackOrb(_diceOrb, false);
                    }
                }
            }
        }

        _diceContainDictionary[_currentDiceSO.DiceSidedType].Roll();
        TurnManager.Instance.DiceRoll();
    }

    private EAudioName GetAudioName(DeBuffEffectType buffName)
    {
        foreach (EAudioName item in Enum.GetValues(typeof(EAudioName)))
        {
            if (item.ToString() == buffName.ToString())
                return item;
        }

        return EAudioName.None;
    }

    private EAudioName GetAudioName(BuffEffectType buffName)
    {
        foreach (EAudioName item in Enum.GetValues(typeof(EAudioName)))
        {
            if (item.ToString() == buffName.ToString())
                return item;
        }

        return EAudioName.None;
    }

    private void DebuffDictionaryInit()
    {
        _debuffDictionary = new Dictionary<DeBuffEffectType, Debuff>();
        foreach (DeBuffEffectType debuffType in Enum.GetValues(typeof(DeBuffEffectType)))
        {
            if (debuffType == DeBuffEffectType.None) continue;
            string typeName = debuffType.ToString();
            try
            {
                Type t = Type.GetType($"{typeName}Debuff");
                Debuff debuff = Activator.CreateInstance(t) as Debuff;
                debuff.Init();
                _debuffDictionary.Add(debuffType, debuff);
            }
            catch
            {
                Debug.LogError($"{debuffType} is Error");
            }
        }

        _buffDictionary = new Dictionary<BuffEffectType, Buff>();
        foreach (BuffEffectType debuffType in Enum.GetValues(typeof(BuffEffectType)))
        {
            if (debuffType == BuffEffectType.None) continue;
            string typeName = debuffType.ToString();
            try
            {
                Type t = Type.GetType($"{typeName}Buff");
                Buff debuff = Activator.CreateInstance(t) as Buff;
                debuff.Init();
                _buffDictionary.Add(debuffType, debuff);
            }
            catch
            {
                Debug.LogError($"{debuffType} is Error");
            }
        }
    }

    public Debuff GetDebuff(DeBuffEffectType type) => _debuffDictionary[type];
    public Buff GetBuff(BuffEffectType type) => _buffDictionary[type];


    public void ChangeDice(DiceSO dice)
    {
        if (!_canDiceChange)
            return;

        if (_currentDiceSO != null)
            _diceContainDictionary[_currentDiceSO.DiceSidedType].gameObject.SetActive(false);
        _currentDiceSO = dice;
        _diceContainDictionary[_currentDiceSO.DiceSidedType].gameObject.SetActive(true);
        ChangeDiceEvent?.Invoke(dice);
    }

    void ColorDictionaryInit()
    {
        _diceColorDictionary = new Dictionary<DiceRareType, Color>();

        int loopCount = _diceColorListSO.DiceColorSOList.Count;
        var colorList = _diceColorListSO.DiceColorSOList;

        for (int i = 0; i < loopCount; ++i)
        {
            if (_diceColorDictionary.ContainsKey(colorList[i].DiceRareType))
                continue;

            _diceColorDictionary.Add(colorList[i].DiceRareType, colorList[i].Color);
        }
    }

    public Color GetDiceRareColor(DiceRareType value)
    {
        return _diceColorDictionary[value];
    }

    private void HandleDiceChangeOrbInit(DiceSO diceSo)
    {
        if (diceSo.BuffType == BuffEffectType.None && diceSo.DeBuffType == DeBuffEffectType.None)
        {
            if (_diceOrb != null)
                _diceOrb.gameObject.SetActive(false);
            return;
        }

        if (_diceOrb != null)
            _diceOrb.gameObject.SetActive(false);

        Vector3 spawnPos = _diceContainDictionary[_currentDiceSO.DiceSidedType].transform.position;
        spawnPos.z = -5f;
        _diceOrb = PoolManager.SpawnFromPool<DiceBezierParent>("BezierParent", spawnPos);
        _diceOrb.Init(diceSo);
    }
}