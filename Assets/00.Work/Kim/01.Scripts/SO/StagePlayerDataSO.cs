using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ayun/StagePlayerData")]
public class StagePlayerDataSO : ScriptableObject
{
    public int col, row;
#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
