using System.Net.NetworkInformation;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    private float _offset;

    private bool _isMoving = true;

    private MenuPlayer _player;
    private MeshRenderer _render;

    private void Awake()
    {
        _player = FindObjectOfType<MenuPlayer>();
        _player.OnRunChange += HandleOnRunChange;
        _render = GetComponent<MeshRenderer>();
    }

    public void HandleOnRunChange(bool isMoving)
    {
        _isMoving = isMoving;
    }

    private void Update()
    {
        if (_isMoving)
        {
            _offset = _offset + Time.deltaTime * _moveSpeed;
            _render.material.mainTextureOffset = new Vector2(_offset, 0);
        }
    }
}
