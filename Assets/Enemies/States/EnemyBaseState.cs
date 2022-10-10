using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyBaseState : BasicState
{
    protected EnemyController _controller;
    protected EnemyBaseState _currentState;
    protected Transform _target;

    protected float nextWaypointDistance = 1;
    protected int currentWaypoint = 0;
    protected float lastRepath = 0f;
    protected bool reachedEndOfPath;

    public EnemyBaseState(EnemyController controller)
    {
        _controller = controller;

        _controller.GetAISensor.OnTargetFound += SetTarget;
        _controller.GetAISensor.OnTargetLost += RemoveTarget;
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        OnExit();

        newState.OnEnter();

        _controller.CurrentState = newState;
    }

    protected void SetTarget()
    {
        _target = _controller.GetAISensor.GetTarget;
    }

    protected void RemoveTarget()
    {
        _target = null;
    }

    protected void OnPathComplete(Path p)
    {
        p.Claim(_controller);

        if (!p.error)
        {
            if (_controller.Path != null)
                _controller.Path.Release(_controller);

            _controller.Path = p;
            currentWaypoint = 0;
        }
        else
        {
            p.Release(_controller);
        }
    }
    protected virtual void HandleMovement()
    {
        if (_controller.Path == null)
            return;

        reachedEndOfPath = false;

        float distanceToWaypoint;

        while (true)
        {
            distanceToWaypoint = Vector3.Distance(_controller.transform.position, _controller.Path.vectorPath[currentWaypoint]);

            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < _controller.Path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        Vector3 direction = (_controller.Path.vectorPath[currentWaypoint] - _controller.transform.position).normalized;
        Vector3 velocity = direction * _controller.MovementSpeed;

        _controller.transform.forward = direction;
        _controller.Controller.Move(velocity * Time.fixedDeltaTime);
    }
}
