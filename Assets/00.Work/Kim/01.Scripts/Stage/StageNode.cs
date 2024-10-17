using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageNode : MonoBehaviour
{
    [SerializeField] private Button _stageButton;
    public List<StageNode> parentNodes = new List<StageNode>();

    public int row;
    public int col;

    private StageSO _stageSO;
    public StageSO StageSO { get { return _stageSO; } }

    private StagePlayer _player;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _player = GetComponentInParent<StageSpawner>().player;
        _stageButton.onClick.AddListener(OnStageClicked);
    }

    private void OnStageClicked()
    {
        Debug.Log("Stage " + _stageSO.stageType + " clicked!");
        _player.MoveStage(CheckMoveStage());
    }

    private StageNode CheckMoveStage()
    {
        if (_player.currentStage != null)
        {
            foreach (StageNode node in _player.currentStage.parentNodes)
            {
                if (node == this)
                    return this;
            }
        }

        return null;
    }

    public void StageNodeInfo(StageSO stageSO)
    {
        _stageSO = stageSO;

        if (_stageSO != null)
        {
            _spriteRenderer.sprite = _stageSO.stageSprite;
        }
    }
}
