using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyController controller) : base (controller) { }

    public override void CheckSwitchStates()
    {
        if (_target)
        {
            SwitchState(_controller.GetState("Follow"));
            return;
        }
    }

    public override void OnEnter()
    {
        _controller.Animator.Play("Attack");
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
}
