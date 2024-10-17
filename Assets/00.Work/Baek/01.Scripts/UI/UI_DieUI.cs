using UnityEngine;
using UnityEngine.UI;
public class UI_DieUI : UI_Popup
{
    [SerializeField]
    private Button _exit;
    protected override void Awake()
    {
        base.Awake();
        _exit.onClick.AddListener(HandleExitEvent);
    }

    private void HandleExitEvent()
    {
        Debug.Log("����");
        ActiveWindow(false);
        SceneControlManager.FadeOut(() => Application.Quit());
    }
}
