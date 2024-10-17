using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[Serializable]
public class EnemyByLevel
{
    public List<GameObject> levelInEnemys;
}

public class EnemySpawn : MonoSingleton<EnemySpawn>
{
    [SerializeField] private List<EnemyByLevel> enemyjungleLevelList;
    [SerializeField] private List<EnemyByLevel> enemysDesertLevelList;
    [SerializeField] private List<EnemyByLevel> enemysMountainLevelList;

    public Queue<Enemy> waveEnemies;

    [SerializeField] private int minEnemySpawnCount;
    [SerializeField] private int maxEnemySpawnCount;

    [SerializeField] private Vector2 spawnPos;
    [SerializeField] private float movePosX;
    [SerializeField] private float startMovePosX;

    private Queue<SpriteRenderer[]> _enemySpriters;
    private Dictionary<SpriteRenderer, Color> _defaultColors;

    private int _waveEnemyCount;


    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        waveEnemies = new Queue<Enemy>();
        _enemySpriters = new Queue<SpriteRenderer[]>();

        _defaultColors = new Dictionary<SpriteRenderer, Color>();
    }

    private void Start()
    {
        SpawnWaveEnemy();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnWaveEnemy();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CheckSortingEnemy();
        }
    }

    private List<GameObject> CurrentLevel(EnemyPage level, Thema thema)
    {
        if(thema == Thema.Jungle)
            return enemyjungleLevelList[(int)level].levelInEnemys;
        if(thema == Thema.Mountain)
            return enemysMountainLevelList[(int)level].levelInEnemys;
        return enemysDesertLevelList[(int)level].levelInEnemys;
    }

    // 적소환
    public void SpawnWaveEnemy()
    {
        GameObject[] currentLevelEnemys = CurrentLevel(StageManager.Instance.GetPage(), 
            StageManager.Instance.currentThema).ToArray();
        
        if (waveEnemies.Count > 0) return;
        
        if (StageManager.Instance.GetPage() == EnemyPage.PageBoss)
            _waveEnemyCount = 1;
        else
            _waveEnemyCount = Random.Range(minEnemySpawnCount, maxEnemySpawnCount + 1);

        for (int i = 0; i < _waveEnemyCount; i++)
        {
            /*풀링하면 바꾸기*/
            int randEnemyNumber = Random.Range(0, currentLevelEnemys.Length);
            GameObject item = Instantiate(currentLevelEnemys[randEnemyNumber], Vector2.right * startMovePosX, Quaternion.identity, transform);

            Enemy enemy = item.GetComponent<Enemy>();
            SpriteRenderer[] sptr = item.transform.Find("Visual")?.GetComponentsInChildren<SpriteRenderer>();

            //적 스탯 정해주기

            _enemySpriters.Enqueue(sptr);
            waveEnemies.Enqueue(enemy);
        }

        foreach (SpriteRenderer[] sprite in _enemySpriters)
        {
            foreach (var item in sprite)
            {
                if (item.color != Color.white)
                    _defaultColors[item] = item.color;
            }
        }

        SortingEnemyColorChange(_enemySpriters.ToList());

        StartCoroutine(SortingEnemyMoveCoroutine());
    }

    IEnumerator SortingEnemyMoveCoroutine()
    {
        int idx = 0;
        while (waveEnemies.Count > idx)
        {
            waveEnemies.ToList()[idx].transform.DOMoveX(spawnPos.x, 0.5f)
                .SetEase(Ease.OutBounce)
                .OnComplete(() => spawnPos = new Vector2(spawnPos.x + Mathf.Abs(movePosX), spawnPos.y));
            yield return new WaitForSeconds(0.5f);
            idx++;
        }
        TurnManager.Instance.BattleStart();
    }

    private void SortingEnemy()
    {
        if (waveEnemies.Count == 0 /*  || waveEnemies.Count <=  */) return;

        waveEnemies.ToList().ForEach(item =>
            item.transform.DOMoveX(item.transform.position.x + movePosX, 1).SetEase(Ease.OutQuint));

        SortingEnemyColorChange(_enemySpriters.ToList());
    }

    private void SortingEnemyColorChange(List<SpriteRenderer[]> enemysColors)
    {
        float nextColor = 1f;

        foreach (var item in enemysColors)
        {
            Color newColor = new Color(1, 1, 1) * nextColor;
            newColor = new Color(newColor.r, newColor.g, newColor.b, 1);

            foreach (var colSpir in item)
            {
                if(colSpir.color == Color.white)
                    colSpir.DOColor(newColor, 1).SetEase(Ease.OutQuint);
            }

            if (nextColor > 0) nextColor -= 0.2f;
        }
    }

    public void CheckSortingEnemy()
    {
        if (waveEnemies.Count > 0 && GetEnemy().isDead) 
        {
            waveEnemies.Dequeue();
            _enemySpriters.Dequeue();
            if(GetEnemy() != null)
                GetEnemy().HealthBarCompo.HandleActiveHpBar(true);
            SortingEnemy();
        }
    }
    
    public Enemy GetEnemy(int addValue = 0)
    {
        List<Enemy> enemyArray = waveEnemies.ToList();
        if (enemyArray.Count <= 0)
            return null;

        return enemyArray[addValue];
    }
}