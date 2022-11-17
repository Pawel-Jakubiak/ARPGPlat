using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : BaseController, IDamageSource, IDamageable
{
    public ItemObject item;

    private PlayerInputActions _input;

    private bool _isJumpPressed = false;
    private bool _isAttackPressed = false;
    private Vector2 _movementInput = Vector2.zero;
    public float comboDelay = .8f;

    public UnityEvent testAction;

    private PlayerBaseState _currentState;
    private Dictionary<string, PlayerBaseState> _states;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; }}

    public bool IsJumpPressed { get { return _isJumpPressed; }}
    public bool IsAttackPressed { get { return _isAttackPressed; } set { _isAttackPressed = value; } }
    public bool IsMoving { get { return _movementInput != Vector2.zero ? true : false; } }
    public Vector2 CurrentMovementInput { get { return _movementInput; } }

    public new int CurrentHealth { 
        get { return _healthPoints; }
        set
        {
            _healthPoints = value;
            HealthbarController.Instance.SetHealthbar(value, _maxHealthPoints);
        }
    }

    protected override void Awake()
    {
        base.Awake();

        InitializeStates();

        _input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _input.Player.Enable();

        _input.Player.Jump.started += ProcessJump;
        _input.Player.Jump.canceled += ProcessJump;

        _input.Player.Move.started += ProcessMovement;
        _input.Player.Move.performed += ProcessMovement;
        _input.Player.Move.canceled += ProcessMovement;

        _input.Player.Attack.started += ProcessAttack;
        _input.Player.Attack.canceled += ProcessAttack;

        HealthbarController.Instance.SetHealthbar(CurrentHealth, MaxHealth);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _currentState?.OnUpdate();

        _controller.Move(_appliedVelocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        //if (_input.Player.Attack.triggered) _isAttackPressed = true;

        if (_input.Player.TestAction.triggered) testAction.Invoke();
    }

    private void ProcessJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
    }

    private void ProcessMovement(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    private void ProcessAttack(InputAction.CallbackContext context)
    {
        _isAttackPressed = context.ReadValueAsButton();
    }

    public void OnHit(DamageInfo damageObject)
    {
        CurrentHealth -= 2;
    }

    public int CalculateDamage()
    {
        return UnityEngine.Random.Range(1, 5);
    }

    public PlayerBaseState GetState(string stateName)
    {
        return _states[stateName];
    }

    private void InitializeStates()
    {
        _states = new Dictionary<string, PlayerBaseState>();

        _states["Grounded"] = new PlayerGroundedState(this);
        _states["Jump"] = new PlayerJumpState(this);
        _states["Fall"] = new PlayerFallState(this);
        _states["Attack"] = new PlayerAttackState(this);
        _states["AirAttack"] = new PlayerAirAttackState(this);

        _currentState = GetState("Grounded");
        _currentState.OnEnter();
    }
}
