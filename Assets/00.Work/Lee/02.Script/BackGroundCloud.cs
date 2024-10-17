using System;
using UnityEngine;

public class BackGroundCloud : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _offset;
    private SpriteRenderer _spriteRenderer;
    private float _maxX;
    private Vector3 _resetPos;

    private void Awake()
    {
        _resetPos = transform.localPosition;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _maxX = _spriteRenderer.sprite.texture.width /_spriteRenderer.sprite.pixelsPerUnit - _offset;
    }

    private void Update()
    {
        if (transform.localPosition.x <= -_maxX)
        {
            _resetPos.x = _maxX;
            transform.localPosition = _resetPos;
        }
        transform.localPosition += Vector3.left * (_speed * Time.deltaTime);
    }
}
