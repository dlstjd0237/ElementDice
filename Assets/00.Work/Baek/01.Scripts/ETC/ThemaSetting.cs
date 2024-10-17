using UnityEngine;
using static Define;
public class ThemaSetting : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _temaObjs;
    private void Awake()
    {
        switch (StageManager.Instance.currentThema)
        {
            case Thema.Jungle:
                _temaObjs[0].SetActive(true);
                break;
            case Thema.Desert:
                _temaObjs[1].SetActive(true);
                break;
            case Thema.Mountain:
                _temaObjs[2].SetActive(true);
                break;
        }


    }
}
