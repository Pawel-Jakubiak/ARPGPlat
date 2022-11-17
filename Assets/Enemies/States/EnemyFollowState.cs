using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyBaseState
{
    protected float lastRepath;

    public EnemyFollowState(EnemyController controller) : base(controller)
    {
    }

    public override void CheckSwitchStates()
    {
        if (_target == null)
        {
            SwitchState(_controller.GetState("Idle"));
            return;
        }

        float distance = Vector3.Distance(_controller.transform.position, _target.transform.position);

        if (distance <= 1.2f)
        {
            SwitchState(_controller.GetState("Attack"));
        }
    }

    public override void OnEnter()
    {
        _controller.GetSeeker.StartPath(_controller.transform.position, _target.position, OnPathComplete);

        lastRepath = Time.time;

        _controller.Animator.Play("RunForward");
    }

    public override void OnUpdate()
    {
        if (_target)
        {
            if (Time.time > lastRepath + _controller.RepathRate || reachedEndOfPath)
            {
                _controller.GetSeeker.StartPath(_controller.transform.position, _target.position, OnPathComplete);

                lastRepath = Time.time;
            }
        }

        HandleMovement();
        CheckSwitchStates();
    }

    public override void OnExit()
    {
    }
}
