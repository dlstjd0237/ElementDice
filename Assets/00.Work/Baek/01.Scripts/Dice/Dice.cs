using System;
using UnityEngine;

public class Dice : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private DiceSidedType _diceSidedType;
    public DiceSidedType DiceSidedType => _diceSidedType;
    private Animation _animation;
    private Material _mat;
    private void Awake()
    {
        _animation = GetComponent<Animation>();
        _mat = _meshRenderer.material;
    }
    private void OnEnable()
    {
        DiceManager.Instance.ChangeDiceEvent += HandleDiecChangeEvent;
    }

    private void OnDestroy()
    {
        DiceManager.Instance.ChangeDiceEvent -= HandleDiecChangeEvent;
    }
    private void HandleDiecChangeEvent(DiceSO so)
    {
        _mat.mainTexture = so.Texture;
    }
    public void Roll()
    {
        _animation.Play();

    }
    public void EndRoll()
    {
        DiceManager.Instance.IsRoll = false;
        DiceManager.Instance.DiceAnimationEnd();
    }
}
