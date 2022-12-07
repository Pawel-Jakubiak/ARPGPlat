using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    private bool _isInRunAnimation;

    public PlayerGroundedState(PlayerController player) : base(player) 
    {
    }


    public override void CheckSwitchStates()
    {
        if (_player.IsAttackPressed)
        {
            SwitchState(_player.GetState("Attack"));
            return;
        }

        if (_player.IsJumpPressed)
        {
            SwitchState(_player.GetState("Jump"));
            return;
        }

        if (!_player.IsGrounded)
        {
            SwitchState(_player.GetState("Fall"));
        }
    }

    public override void OnEnter()
    {
        if (_player.IsMoving)
        {
            _player.Animator.CrossFade("RunForward", .1f);
            _isInRunAnimation = true;
        }
        else
        {
            _player.Animator.CrossFade("Idle", .1f);
            _isInRunAnimation = false;
        }

        _player._appliedVelocity.y = _player._currentVelocity.y = _player.groundedGravity;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        HandleMovement();

        if (_player.IsMoving && !_isInRunAnimation)
        {
            _player.Animator.CrossFade("RunForward", .1f);
            _isInRunAnimation = true;
        }
        else if (!_player.IsMoving && _isInRunAnimation)
        {
            _player.Animator.CrossFade("Idle", .1f);
            _isInRunAnimation = false;
        }

        CheckSwitchStates();
    }
}
