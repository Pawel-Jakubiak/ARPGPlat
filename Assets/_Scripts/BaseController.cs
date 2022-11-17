using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField] protected int _maxHealthPoints;
    [SerializeField] protected int _healthPoints;

    [SerializeField] protected float _movementSpeed;
    protected CharacterController _controller;
    protected Animator _animator;

    public Vector3 _currentVelocity = Vector3.zero;
    public Vector3 _appliedVelocity = Vector3.zero;

    public float gravity;
    public float groundedGravity = -0.1f;

    [SerializeField] protected float _jumpTime;
    [SerializeField] protected float _jumpHeight;
    [SerializeField] protected float _fallSpeed;
    protected bool _isGrounded;

    public CharacterController Controller { get { return _controller; } }
    public Animator Animator { get { return _animator; } }
    public bool IsGrounded { get { return _isGrounded; } }
    public int CurrentHealth { get { return _healthPoints; } set { _healthPoints = value; } }
    public int MaxHealth { get { return _maxHealthPoints; } }
    public float MovementSpeed { get { return _movementSpeed; } set { _movementSpeed = value; } }
    public float JumpTime { get { return _jumpTime; } set { _jumpTime = value; } }
    public float JumpHeight { get { return _jumpHeight; } set { _jumpHeight = value; } }
    public float FallSpeed { get { return _fallSpeed; } set { _fallSpeed = value; } }

    protected virtual void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        CurrentHealth = MaxHealth;
    }

    protected virtual void FixedUpdate()
    {
        IsControllerGrounded();
    }

    private void IsControllerGrounded()
    {
        RaycastHit hit;
        Vector3 castPosition = transform.position + Vector3.up * (Physics.defaultContactOffset + _controller.radius);

        Physics.SphereCast(castPosition, _controller.radius, Vector3.down, out hit);

        if (hit.collider)
        {
            _isGrounded = hit.distance <= .1f ? true : false;
        }
    }
}
