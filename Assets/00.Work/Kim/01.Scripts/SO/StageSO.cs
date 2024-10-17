using UnityEngine;

public enum StageType
{
    None, Start, Battle, Store, Boss
}

[CreateAssetMenu(menuName = "SO/Ayun/Stage")]
public class StageSO : ScriptableObject
{
    public StageType stageType;
    public Sprite stageSprite;
}
