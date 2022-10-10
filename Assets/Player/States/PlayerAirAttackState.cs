using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirAttackState : PlayerBaseState
{
    public PlayerAirAttackState(PlayerController player) : base(player) 
    {
        //redSpeed = .5f;
    }

    private bool animationEnded = false;
    private bool inAttackAnimation = false;
    private int comboPhase = 0;

    public override void CheckSwitchStates()
    {
        if (animationEnded && (comboPhase == 3 || !_player.IsAttackPressed))
            SwitchState(_player.GetState("Fall"));
    }

    public override void OnEnter()
    {
        _player._currentVelocity = _player._appliedVelocity = Vector3.zero;

        CommenceAttack();
    }

    public override void OnExit()
    {
        comboPhase = 0;
        _player.IsAttackPressed = false;
    }

    public override void OnUpdate()
    {
        float animationTimer = _player.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

        animationEnded = animationTimer >= 1 ? true : false;
        inAttackAnimation = animationTimer <= _player.comboDelay ? true : false;

        if (!inAttackAnimation)
        {
            HandleMovement();

            _player._currentVelocity.y = _player._appliedVelocity.y += -2f;
        }
        else
        {
            Vector3 playerForward = _player.transform.forward;

            _player._currentVelocity.x = _player._appliedVelocity.x = playerForward.x * 2;
            _player._currentVelocity.z = _player._appliedVelocity.z = playerForward.z * 2;
        }

        if (!inAttackAnimation && _player.IsAttackPressed && comboPhase < 3)
            CommenceAttack();

        CheckSwitchStates();
    }

    private void CommenceAttack()
    {
        comboPhase += 1;
        _player.IsAttackPressed = animationEnded = false;
        _player.Animator.Play("Attack" + comboPhase);
    }
}
