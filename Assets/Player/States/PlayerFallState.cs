using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState
{
    public PlayerFallState(PlayerController player) : base(player) { }

    public override void CheckSwitchStates()
    {
        if (_player.IsGrounded)
        {
            SwitchState(_player.GetState("Grounded"));
        }
    }

    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        HandleGravity();
        HandleMovement();
        CheckSwitchStates();
    }

    private void HandleGravity()
    {
        float previousYVelocity = _player._currentVelocity.y;
        _player._currentVelocity.y = previousYVelocity + (_player.gravity * _player.FallSpeed * Time.deltaTime);

        float appliedYVelocity = Mathf.Max(previousYVelocity + _player._currentVelocity.y, -20);
        _player._appliedVelocity.y = appliedYVelocity;
    }
}
