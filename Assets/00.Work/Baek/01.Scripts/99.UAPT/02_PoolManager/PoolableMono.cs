using UnityEngine;

public class PoolableMono : MonoBehaviour
{
    protected virtual void Awake() => Init();
    protected virtual void Init() { }
    protected virtual void OnDisable() =>
    PoolManager.ReturnToPool(gameObject);
}
