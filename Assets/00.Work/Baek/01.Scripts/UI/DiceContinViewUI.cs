using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UAPT.Utile;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
public class DiceContinViewUI : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset _diceCardTree;
    [SerializeField] private List<DiceCardListSO> _diceCardSOList;
    [SerializeField] private InputReader _input;
    [SerializeField] private ChoiceCheckBoxUI _checkBox;

    private Dictionary<string, DiceCard> _diceCardDictionary;

    private DiceSO _currentChiceDiceSO;

    private UIDocument _doc;

    private VisualElement _root;
    private VisualElement _containBox;

    private Label _diceDescriptLabel;

    private Button _cancelBtn;
    private Button _equipBtn;
    private Button _prior;

    private ScrollView _diceScrollView;

    private void Awake()
    {
        _diceCardDictionary = new Dictionary<string, DiceCard>();
        _doc = GetComponent<UIDocument>();
        _root = _doc.rootVisualElement;
    }

    private void OnEnable()
    {
        _containBox = _root.Q<VisualElement>("contain-box");
        _diceScrollView = _root.Q<ScrollView>("dice-scroll_view");
        _cancelBtn = _root.Q<Button>("cancel-btn");
        _diceDescriptLabel = _root.Q<Label>("dice_effect_descript-label");
        _equipBtn = _root.Q<Button>("equip-btn");
        _equipBtn.RegisterCallback<ClickEvent>(HandleEquipEvent);

        _cancelBtn.clicked += HandleCancelEvent;

        ToolKitUtile.SetScrollSpeed(_diceScrollView, 800, ScrollDirection.Horizontal);

        _input.DiceListViewEvent += HandleDiceViewOn;
    }
    private void Start()
    {
        DiceInit();

    }
    private void DiceInit()
    {
        var SODictionary = DiceManager.Instance.DiceCardDictionary;

        foreach (var item in SODictionary)
        {
            Debug.Log(item.Value.DiceName);
            DiceSO SO = item.Value;
            int Amount = DiceManager.Instance.DiceAmountDictionary[SO.DiceName];
            for (int i = 0; i < Amount + 1; ++i)
            {
                AddDice(SO);
            }
        }
    }

    private void HandleCancelEvent()
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);
        _containBox.ToggleInClassList("on");
        if (_containBox.ClassListContains("on"))
        {
            _input.Console.RunTime.Enable();
            _input.Console.UI.ESC.Enable();
        }
        else
        {
            _input.Console.RunTime.Disable();
            _input.Console.UI.ESC.Disable();
        }
    }

    private void HandleEquipEvent(ClickEvent evt)
    {
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Click);

        if (_currentChiceDiceSO != null)
            DiceManager.Instance.ChangeDice(_currentChiceDiceSO);
    }

    private void OnDestroy()
    {
        _input.DiceListViewEvent -= HandleDiceViewOn;
    }
    
    public void HandleDiceViewOn()
    {
        _containBox.ToggleInClassList("on");
        
        if (_containBox.ClassListContains("on"))
        {
            _input.Console.RunTime.Enable();
            _input.Console.UI.ESC.Enable();
        }
        else
        {
            _input.Console.RunTime.Disable();
            _input.Console.UI.ESC.Disable();
        }
    }
    
    public void AddDice(DiceSO so)
    {
        DiceManager.Instance.AddDiceCardDictionary(so);


        Debug.Log(_diceCardDictionary.ContainsKey(so.DiceName));
        
        if (!_diceCardDictionary.ContainsKey(so.DiceName))
        {
            var templat = _diceCardTree.Instantiate().Q<VisualElement>();
            var diceNameLabel = templat.Q<Label>("dice_name-label");
            var diceBtn = templat.Q<Button>("dice_card-contain-btn");

            diceBtn.RegisterCallback<MouseEnterEvent>(evt =>
            {
                _diceDescriptLabel.text = so.DiceDescript;
            });
            diceBtn.RegisterCallback<MouseOutEvent>(evt =>
            {
                _diceDescriptLabel.text = string.Empty;
            });
            diceBtn.RegisterCallback<MouseMoveEvent>(evt =>
            {
                if (_diceDescriptLabel.text == string.Empty)
                {
                    _diceDescriptLabel.text = so.DiceDescript;
                }
            });
            diceBtn.RegisterCallback<ClickEvent>(evt =>
            {
                _currentChiceDiceSO = so;
                CardChoiceEffect(so.DiceName);
            });

            diceNameLabel.text = so.DiceName;
            diceNameLabel.style.color = new StyleColor(DiceManager.Instance.GetDiceRareColor(so.DiceRareType));

            _diceScrollView.Add(templat);

            var diceCard = new DiceCard(templat, so, _checkBox, this);
            _diceCardDictionary.Add(so.DiceName, diceCard);
        }
        else//��ųʸ��� ���� �� ���
        {
            var dice = _diceCardDictionary[so.DiceName];
            Debug.Log(DiceManager.Instance.DiceAmountDictionary[so.DiceName]);
            if (dice.AddAmount() && dice.CurrentSO.NextLevelDiceSo != null)//true �� ������ ��ȭ �����Ѱ���
            {
                dice.CanLevelUp = true;
            }
        }
    }
    private void CardChoiceEffect(string choiceDiceName)
    {
        if (_prior != null)
            _prior.RemoveFromClassList("on");

        Button btn = _diceCardDictionary[choiceDiceName].CardBtn;
        btn.AddToClassList("on");
        _prior = btn;
    }
}
