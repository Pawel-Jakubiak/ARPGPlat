using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyBaseState
{
    public EnemyHitState(EnemyController controller) : base (controller) { }

    public override void CheckSwitchStates()
    {
        if (_controller.CurrentHealth <= 0)
        {
            SwitchState(_controller.GetState("Death"));
            return;
        }

        if (_target)
        {
            SwitchState(_controller.GetState("Follow"));
            return;
        }
    }

    public override void OnEnter()
    {
        _controller.Animator.Play("Hit");
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        if (_controller.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            return;

        CheckSwitchStates();
    }

    public override void OnHit(DamageInfo damageInfo)
    {
        _controller.CurrentHealth = (int)Mathf.Clamp(_controller.CurrentHealth - damageInfo.damage, 0f, _controller.MaxHealth);

        _controller.Animator.Play("Hit", -1, 0);
    }
}
