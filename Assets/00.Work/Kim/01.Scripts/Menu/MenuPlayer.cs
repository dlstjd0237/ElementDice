using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MenuPlayer : MonoBehaviour
{
    public Action<bool> OnRunChange = null;

    private float _currentTime;
    private float _changeTime = 7f;

    private Animator _animator;

    #region Attack
    [SerializeField] private float _moveTime;
    [SerializeField] private float _returnTime;
    [SerializeField] private float _attackPosOffset;
    [SerializeField] private Vector3 _defaultPos;

    private float _attackPosX;
    private Enemy _currentEnemy;

    private Tween _runTween;
    #endregion

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        RunSetting(true);
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > _changeTime)
        {
            _currentTime = 0;
            _changeTime = Random.Range(7, 15);
            RandomAnimation(Random.Range(1, 4)); // 1~3
        }
    }

    private void RandomAnimation(int rand)
    {
        switch(rand)
        {
            case 1: // Attack
                AttackStart();
                break;
            case 2: // Dead
                DeadStart();
                break;
            case 3: // Stun
                StartStun();
                break;
        }
    }

    #region Attack
    private void AttackStart()
    {
        _attackPosX = 4 - _attackPosOffset;
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        Running(_attackPosX, _moveTime);
        yield return _runTween.WaitForCompletion();
        yield return new WaitForSeconds(1f);
        RunSetting(false);
        _animator.SetTrigger("Attack");

        yield return new WaitForSeconds(1.5f);

        RunSetting(true);
        Running(_defaultPos.x, _returnTime);
        yield return _runTween.WaitForCompletion();
    }

    private void Running(float pos, float time)
    {
        _runTween = transform.DOMoveX(pos, time).SetEase(Ease.Linear);
    }
    #endregion

    #region Dead
    private void DeadStart()
    {
        StartCoroutine(DeadRoutine());
    }

    private IEnumerator DeadRoutine()
    {
        RunSetting(false);
        _animator.SetBool("isDead", true);
        yield return new WaitForSeconds(2f);
        _animator.SetBool("isDead", false);
        yield return new WaitForSeconds(2f);
        RunSetting(true);
    }
    #endregion

    #region Stun
    private void StartStun()
    {
        StartCoroutine(StunRoutine());
    }

    private IEnumerator StunRoutine()
    {
        RunSetting(false);
        _animator.SetBool("isStun", true);
        yield return new WaitForSeconds(3f);
        _animator.SetBool("isStun", false);
        yield return new WaitForSeconds(1.5f);
        RunSetting(true);
    }
    #endregion

    private void RunSetting(bool isRun)
    {
        _animator.SetBool("isRun", isRun);
        OnRunChange?.Invoke(isRun);
    }
}
