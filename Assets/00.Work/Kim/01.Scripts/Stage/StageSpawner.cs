using System.Collections.Generic;
using UnityEngine;

public class StageSpawner : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private GameObject _stageNodePrefab;
    [SerializeField] private GameObject _lineRendererPrefab;
    [SerializeField] private Transform _lineSpawnTrm;
    [SerializeField] private List<StageSO> _stageInfoList = new List<StageSO>();

    [Header("Stage Spawn Setting")]
    [SerializeField] private float _horizontalSpacing; // 가로 간격
    [SerializeField] private float _verticalSpacing; // 세로 간격

    private int _storeRowSpawnNum; // 가로줄이 얼마나 지나야 상점 줄이 생성되는지
    private int _numRow;  // 가로로 몇 단계 뻗어 나갈지
    private int _maxNodesPerCol; // 최대 세로 노드 숫자

    [HideInInspector] public StagePlayer player;
    [HideInInspector] public List<StageNode> stageNodes;

    private void Awake()
    {
        player = FindObjectOfType<StagePlayer>();
        stageNodes = new List<StageNode>();
    }

    private void Start()
    {
        DiceManager.Instance.DiceNumTextActive(false);
        SpawnSetting();
        GenerateStages();
    }

    private void SpawnSetting()
    {
        _storeRowSpawnNum = StageManager.Instance.currentThemaSO._storeRowSpawnNum;
        _numRow = StageManager.Instance.currentThemaSO._numRow;
        _maxNodesPerCol = StageManager.Instance.currentThemaSO._maxNodesPerRow;
    }

    private void GenerateStages()
    {
        #region 시작 노드
        StageNode startNode = CreateStageNode(Vector2.zero, 0, 0, StageType.Start);
        player.PlayerStartNodeSet(startNode);
        List<StageNode> currentRowNodes = new List<StageNode> { startNode };
        #endregion

        #region 중간 노드
        for (int row = 1; row <= _numRow; row++)
        {
            List<StageNode> nextRowNodes = new List<StageNode>();

            // 지금 가로에 생성될 노드 수
            int nodesInThisRow = Mathf.Min(row + 1, _maxNodesPerCol);

            StageType spawnStageType = StageType.Battle;
            if (row % (_storeRowSpawnNum + 1) == 0)
                spawnStageType = StageType.Store;

            for (int col = 0; col < nodesInThisRow; col++)
            {
                float xOffset = (col - (nodesInThisRow - 1) / 2.0f) * _horizontalSpacing;
                Vector2 position = new Vector2(xOffset, row * _verticalSpacing);

                StageNode newNode = CreateStageNode(position, row, col, spawnStageType);
                nextRowNodes.Add(newNode);
            }

            ConnectParents(currentRowNodes, nextRowNodes); // 노드 연결
            currentRowNodes = nextRowNodes;
        }
        #endregion

        #region 보스 노드
        Vector2 bossPosition = new Vector2(0, (_numRow + 1) * _verticalSpacing);
        StageNode bossNode = CreateStageNode(bossPosition, _numRow + 1, 0, StageType.Boss);

        foreach (StageNode node in currentRowNodes)
        {
            node.parentNodes.Add(bossNode);
            DrawConnection(node.transform.position, bossNode.transform.position);
        }
        #endregion
    }

    private StageNode CreateStageNode(Vector2 position, int row, int col, StageType stageType)
    {
        GameObject stageObj = Instantiate(_stageNodePrefab, transform);
        stageObj.transform.localPosition = position;

        foreach (StageSO stageSO in _stageInfoList)
        {
            if (stageSO.name == $"{stageType}Stage")
            {
                StageNode stageNode = stageObj.GetComponent<StageNode>();
                stageNode.StageNodeInfo(stageSO);
                stageNode.row = row;
                stageNode.col = col;

                // 플레이어 위치 정하기
                if (player.PlayerRow == stageNode.row && player.PlayerCol == stageNode.col)
                {
                    player.currentStage = stageNode;
                    player.transform.position = stageNode.transform.position;
                }

                stageNodes.Add(stageNode);
                return stageNode;
            }
        }
        return null;
    }

    private void ConnectParents(List<StageNode> currentRowNodes, List<StageNode> nextRowNodes)
    {
        foreach (StageNode currentNode in currentRowNodes)
        {
            // 모든 부모 노드와의 거리 계산
            nextRowNodes.Sort((a, b) =>
                Vector2.Distance(currentNode.transform.position, a.transform.position).CompareTo(
                Vector2.Distance(currentNode.transform.position, b.transform.position)));

            // 부모 노드와 연결
            for (int i = 0; i < 2 && i < nextRowNodes.Count; i++)
            {
                currentNode.parentNodes.Add(nextRowNodes[i]);
                DrawConnection(nextRowNodes[i].transform.position, currentNode.transform.position);
            }
        }
    }

    private void DrawConnection(Vector3 start, Vector3 end)
    {
        GameObject lineObj = Instantiate(_lineRendererPrefab, _lineSpawnTrm);
        LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

        LineDrawer lineDrawer = lineObj.GetComponent<LineDrawer>();
        lineDrawer.lineRenderer = lineRenderer;

        lineDrawer.DrawLine(start, end);
    }
}