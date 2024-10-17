using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceSOCreateEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private TextField _nameField;
    private EnumField _rareField;
    private EnumField _sidedField;
    private EnumField _actionField;
    private EnumField _effectField;
    private Button _createBtn;


    [MenuItem("Tools/Baek/DiceSOCreateTool")]
    public static void ShowExample()
    {
        DiceSOCreateEditor wnd = GetWindow<DiceSOCreateEditor>();
        wnd.titleContent = new GUIContent("DiceSOCreateTool");

        wnd.minSize = new Vector2(350, 340);
        wnd.maxSize = new Vector2(350, 340);
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement UXML = m_VisualTreeAsset.Instantiate();
        root.Add(UXML);

        _nameField = UXML.Q<TextField>("tool_dice_name-field");
        _rareField = UXML.Q<EnumField>("tool_dice_rare-field");
        _sidedField = UXML.Q<EnumField>("tool_dice_sided-field");
        _actionField = UXML.Q<EnumField>("tool_dice_action-field");
        _effectField = UXML.Q<EnumField>("tool_dice_effect-field");
        _createBtn = UXML.Q<Button>("tool_dice_create-btn");


        _createBtn.RegisterCallback<ClickEvent>(HandleCreateSO);
    }

    private void HandleCreateSO(ClickEvent evt)
    {
        if ((_nameField.value == null || _rareField.value == null) || (_sidedField.value == null || _actionField.value == null)
            || (_effectField.value == null || (DiceRareType)_rareField.value == DiceRareType.None))
            return;

        string soName = _nameField.value;
        string fileName = $"Assets/00.Work/DiceSO/{_rareField.value}DiceSO/{soName}SO.asset";

        DiceSO asset = AssetDatabase.LoadAssetAtPath<DiceSO>(fileName);

        if (asset != null)
        {
            asset.DiceName = _nameField.value;
            asset.DiceRareType = (DiceRareType)_rareField.value;
            asset.DiceSidedType = (DiceSidedType)_sidedField.value;
            asset.DiceActionType = (DiceActionType)_actionField.value;
            asset.DeBuffType = (DeBuffEffectType)_effectField.value;
            EditorUtility.SetDirty(asset); //디스크 저장
            AssetDatabase.SaveAssets(); //변경사항 유니티 메모리에 저장
        }
        else
        {
            asset = ScriptableObject.CreateInstance<DiceSO>();

            asset.DiceName = _nameField.value;
            asset.DiceRareType = (DiceRareType)_rareField.value;
            asset.DiceSidedType = (DiceSidedType)_sidedField.value;
            asset.DiceActionType = (DiceActionType)_actionField.value;
            asset.DeBuffType = (DeBuffEffectType)_effectField.value;

            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"Assets/00.Work/DiceSO/{_rareField.value}DiceSO/{soName}.asset");
            Debug.Log(asset);
            AssetDatabase.CreateAsset(asset, fileName); //생성
            AssetDatabase.Refresh(); //변경사항 새로고침
        }
    }
}
