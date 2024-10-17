using System;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoSingleton<CameraManager>
{
    private CinemachineVirtualCamera _camera;
    private Vector3 _defaultPos;

    private void Awake()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        _defaultPos = transform.position;
        SceneManager.sceneLoaded += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }

    private void SceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        _camera = FindObjectOfType<CinemachineVirtualCamera>();
    }


    public void ShakingCamera(float duration, float str)
    {
        _camera.transform.DOShakePosition(duration, str).OnComplete(()=>transform.position=_defaultPos);
    }
}
