using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float lastPatrol;

    public EnemyIdleState(EnemyController controller) : base (controller) { }

    public override void CheckSwitchStates()
    {
        if (Time.time > lastPatrol + _controller.PatrolRate)
        {
            SwitchState(_controller.GetState("Patrol"));
        }
    }

    public override void OnEnter()
    {
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
