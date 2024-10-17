using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyPage
{
    None = 0, 
    PageOne, PageTwo, PageThree, PageFour, PageBoss
}

public enum Thema
{
    None,
    Jungle,
    Desert,
    Mountain
}

public class StageManager : MonoSingleton<StageManager>
{
    [Header("Thema")]
    [SerializeField] private List<ThemaInfoSO> _themaList = new List<ThemaInfoSO>();
    public List<ThemaInfoSO> ThemaList => _themaList;

    [Header("PlayerSO")]
    [SerializeField] private StagePlayerDataSO _playerData;

    [HideInInspector] public ThemaInfoSO currentThemaSO = null;
    [HideInInspector] public Thema currentThema = Thema.Jungle;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
    }

    // 보스 클리어한 테마 SO의 isClear를 true로 바꿔줘야함
    // 실행 해주면 테마 바꿔줌
    public void ThemaChange(Thema thema)
    {
        currentThema = thema;

        foreach (ThemaInfoSO info in _themaList)
        {
            if (info.thema == currentThema)
                currentThemaSO = info;
        }

        _playerData.row = 0;
        _playerData.col = 0;
    }

    public EnemyPage GetPage()
    {
        int pageCnt = currentThemaSO._numRow / 4;
        if (_playerData.row == currentThemaSO._numRow + 1) return EnemyPage.PageBoss;
        for (int i= 1; i <= 4; ++i)
        {
            if (_playerData.row <= (pageCnt * i) || i == 4)
            {
                Debug.Log(i);
                return (EnemyPage)i;
            }
        }

        return EnemyPage.None;
    }
}
