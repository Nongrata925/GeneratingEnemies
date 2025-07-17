using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Renderer))]
public class Enemy : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private Vector3 _movementDirection;
    private float _movementSpeed = 8.0f;
    
    public event Action<Enemy> ReleaseZoneReached;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
    }

    public void Initialize(Vector3 movementDirection)
    {
        _rigidbody.transform.rotation = Quaternion.identity;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _renderer.material.color = Color.red;
        _movementDirection = movementDirection;
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movementDirection * Time.fixedDeltaTime * _movementSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ReleaseZone>(out _))
        {
            ReleaseZoneReached?.Invoke(this);
        }
    }
}