using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum StatusType
{
    Shield,
}

public class StatusHUD : MonoBehaviour
{
    private Image _image;
    private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Init(Sprite image, string text)
    {
        _image.sprite = image;
        _textMeshProUGUI.SetText(text);
    }

    public void Change(string text)
    {
        _textMeshProUGUI.SetText(text);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
