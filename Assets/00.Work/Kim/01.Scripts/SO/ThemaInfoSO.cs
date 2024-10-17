using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ayun/ThemaInfo")]
public class ThemaInfoSO : ScriptableObject
{
    public Thema thema;

    // �������� �󸶳� ������ ���� ���� �����Ǵ���
    public int _storeRowSpawnNum;

    public int _numRow;  // ���η� �� �ܰ� ���� ������
    public int _maxNodesPerRow; // �ִ� ���� ��� ����
}
