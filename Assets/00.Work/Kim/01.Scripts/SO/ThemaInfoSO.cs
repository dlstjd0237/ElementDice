using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ayun/ThemaInfo")]
public class ThemaInfoSO : ScriptableObject
{
    public Thema thema;

    // 세로줄이 얼마나 지나야 상점 줄이 생성되는지
    public int _storeRowSpawnNum;

    public int _numRow;  // 세로로 몇 단계 뻗어 나갈지
    public int _maxNodesPerRow; // 최대 가로 노드 숫자
}
