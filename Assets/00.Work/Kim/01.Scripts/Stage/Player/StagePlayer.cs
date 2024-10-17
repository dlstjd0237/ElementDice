using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePlayer : MonoBehaviour
{
    [SerializeField] private StagePlayerDataSO _stagePlayerData;
    [SerializeField] private float _moveTime;
    public StageNode currentStage;

    private Animator _animator;

    public int PlayerRow { get => _stagePlayerData.row; }
    public int PlayerCol { get => _stagePlayerData.col; }

    private Collider2D nodeCollier;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void MoveStage(StageNode stageNode)
    {
        if (stageNode != null)
        {
            _stagePlayerData.row = stageNode.row;
            _stagePlayerData.col = stageNode.col;

            _animator.SetBool("isRun", true);
            transform.DOMove(stageNode.transform.position, _moveTime)
                .OnComplete(() =>
                {
                    currentStage = stageNode;
                    _animator.SetBool("isRun", false);
                    if (stageNode.StageSO.stageType == StageType.Battle || stageNode.StageSO.stageType == StageType.Boss)
                        SceneControlManager.FadeOut(() => SceneManager.LoadScene("BattleScene"));
                    if (stageNode.StageSO.stageType == StageType.Store)
                        SceneControlManager.FadeOut(() => SceneManager.LoadScene("Shop"));
                });
        }
    }

    public void PlayerStartNodeSet(StageNode stageNode)
    {
        if (stageNode != null)
        {
            if (_stagePlayerData.row == 0 && _stagePlayerData.col == 0)
            {
                currentStage = stageNode;
                _stagePlayerData.row = 0;
                _stagePlayerData.col = 0;
            }
        }
    }
}
