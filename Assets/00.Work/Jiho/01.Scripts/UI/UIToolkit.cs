using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIToolkit : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    
    protected VisualElement Root { get; private set; }

    protected void RootSetting()
    {
        Root = GetComponent<UIDocument>().rootVisualElement;
    }
}