using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BezierTrail : MonoBehaviour
{
    [SerializeField] private float _moveTime;
    [SerializeField] private ParticleSystem _hitEffect;
    private ParticleSystem _hitObj;
    
    private TrailRenderer _trailRenderer;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    public void Init(Color color)
    {
        _particleSystem.startColor = color;
        _trailRenderer.startColor = color;
    }

    public IEnumerator StartBezier(Vector2 startPos, Vector2 secondPos, Vector2 targetPos, Action EndAction, bool isLast)
    {
        float deltaTime = 0;
        float elapse = 0;
        while (elapse <= 1)
        {
            deltaTime += Time.deltaTime;
            elapse = deltaTime / _moveTime;

            Vector2 firstPoint = Vector2.Lerp(startPos, secondPos, elapse);
            Vector2 secondPoint = Vector2.Lerp(secondPos, targetPos, elapse);

            Vector3 currentPoint = Vector2.Lerp(firstPoint, secondPoint, elapse);
            currentPoint.z = -5f;
            transform.position = currentPoint;
            yield return null;
        }
        
        CameraManager.Instance.ShakingCamera(0.2f, 0.2f);
        SoundManager.PlaySound(Define.EAudioType.SFX, Define.EAudioName.Orb);
        HitObjIns();
        
        EndAction?.Invoke();
        gameObject.SetActive(false);
        
        if (isLast)
        {
            yield return DelayTimeManager.Instance.GetDelayTime(1f);
            TurnEventBus.Publish(TurnEnumType.PlayerBuffApply);
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void HitObjIns()
    {
        _hitObj = Instantiate(_hitEffect);
        _hitObj.startColor = _trailRenderer.startColor;
        _hitObj.gameObject.SetActive(false);
        _hitObj.transform.position = transform.position;
        _hitObj.gameObject.SetActive(true);
        _hitObj.Play();
    }
}
