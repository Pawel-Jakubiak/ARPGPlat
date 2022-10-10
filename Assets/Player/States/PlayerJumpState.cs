using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    private LayerMask ceilingMasks;

    public PlayerJumpState(PlayerController player) : base(player)
    {
        ceilingMasks = LayerMask.GetMask("Default");
    }

    public override void CheckSwitchStates()
    {
        if (_player.IsAttackPressed)
        {
            SwitchState(_player.GetState("AirAttack"));
            return;
        }

        if (_player._currentVelocity.y <= 0)
        {
            SwitchState(_player.GetState("Fall"));
            return;
        }

        if (_player.IsGrounded)
        {
            SwitchState(_player.GetState("Grounded"));
        }
    }

    public override void OnEnter()
    {
        _player.Animator.Play("JumpWhileRunning");
        HandleJump();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        HandleGravity();
        HandleMovement();
        CheckForCeiling();
        CheckSwitchStates();
    }

    private void CheckForCeiling()
    {
        bool ceilingHit = Physics.CheckSphere(_player.transform.position + (_player.transform.up * _player.Controller.height * .8f), _player.Controller.radius, ceilingMasks);
        Collider[] test = Physics.OverlapSphere(_player.transform.position + (_player.transform.up * _player.Controller.height * .8f), _player.Controller.radius, ceilingMasks);

        foreach (Collider col in test)
            Debug.Log($"{col.name}");

        if (ceilingHit)
            _player._appliedVelocity.y = _player._currentVelocity.y = 0;
    }

    private void HandleJump()
    {
        float timeToApex = _player.JumpTime / 2;
        float jumpVelocity = (2 * _player.JumpHeight) / timeToApex;

        _player.gravity = (-2 * _player.JumpHeight) / (timeToApex * timeToApex);

        _player._currentVelocity.y = jumpVelocity;
        _player._appliedVelocity.y = jumpVelocity;
    }

    protected virtual void HandleGravity()
    {
        float previousYVelocity = _player._currentVelocity.y;
        _player._currentVelocity.y = previousYVelocity + _player.gravity * Time.fixedDeltaTime;

        float appliedYVelocity = previousYVelocity + _player._currentVelocity.y;
        _player._appliedVelocity.y = appliedYVelocity;
    }
}
