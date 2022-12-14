using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float lastPatrol;

    public EnemyIdleState(EnemyController controller) : base (controller) { }

    public override void CheckSwitchStates()
    {
        if (_target)
        {
            SwitchState(_controller.GetState("Follow"));
            return;
        }

        if (Time.time > lastPatrol + _controller.PatrolRate)
        {
            SwitchState(_controller.GetState("Patrol"));
        }
    }

    public override void OnEnter()
    {
        _controller.Animator.Play("Idle");
        lastPatrol = Time.time;
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        CheckSwitchStates();
    }
}
