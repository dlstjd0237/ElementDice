using UnityEngine;
using Baek;
[RequireComponent(typeof(CanvasGroup))]
public class UI_Popup : MonoBehaviour
{
    protected CanvasGroup _canvasGroup;

    protected virtual void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    public void ActiveWindow(bool isOpen) => Managers.UI.ActiveWindow(_canvasGroup, isOpen);

}
