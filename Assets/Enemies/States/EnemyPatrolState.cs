using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyController controller) : base(controller)
    {
    }

    public override void CheckSwitchStates()
    {
        if (_target)
        {
            SwitchState(_controller.GetState("Follow"));
            return;
        }

        if (reachedEndOfPath)
        {
            SwitchState(_controller.GetState("Idle"));
        }
    }

    public override void OnEnter()
    {
        Vector2 randomPosition = Random.insideUnitCircle * _controller.PatrolRadius;
        
        Vector3 targetPosition = _controller.SpawnPosition;
        targetPosition.x += randomPosition.x;
        targetPosition.z += randomPosition.y;

        _controller.GetSeeker.StartPath(_controller.transform.position, targetPosition, OnPathComplete);

        _controller.Animator.Play("RunForward");
    }

    public override void OnUpdate()
    {
        HandleMovement();
        CheckSwitchStates();
    }

    public override void OnExit()
    {
    }
}
