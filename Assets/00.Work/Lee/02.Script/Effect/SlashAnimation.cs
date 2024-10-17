using UnityEngine;

public class SlashAnimation : MonoBehaviour
{
    private Animator _animator;

    private readonly int _isCriticalHash = Animator.StringToHash("IsCritical");
    private readonly int _isSlashIndexHash = Animator.StringToHash("SlashIndex");
    private readonly int _isSlashTrigger = Animator.StringToHash("SlashTrigger");
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetEffect(bool isCritical, int slashIndex)
    {
        _animator.SetBool(_isCriticalHash, isCritical);
        _animator.SetInteger(_isSlashIndexHash, slashIndex);
    }

    public void PlayEffect() => _animator.SetTrigger(_isSlashTrigger);
    public void EndEffect() => gameObject.SetActive(false);
}
