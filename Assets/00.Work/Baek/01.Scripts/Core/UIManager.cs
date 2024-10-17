using UnityEngine;
using DG.Tweening;
namespace Baek
{
    public class UIManager
    {
        public void ActiveWindow(CanvasGroup canvasGroup, bool isOpen)
        {
            if (canvasGroup == null)
            {
                Debug.LogError("CanvasGroup is Null");
                return;
            }
            int fadeValue = isOpen ? 1 : 0;
            canvasGroup.DOFade(fadeValue, 0.1f);
            canvasGroup.interactable = isOpen;
            canvasGroup.blocksRaycasts = isOpen;
        }
    }
}

