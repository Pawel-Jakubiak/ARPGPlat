using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyBaseState : BasicState
{
    protected EnemyController _controller;
    protected EnemyBaseState _currentState;
    protected Transform _target { get { return _controller.GetAISensor.GetTarget; } }

    protected float nextWaypointDistance = 1;
    protected int currentWaypoint = 0;
    protected bool reachedEndOfPath;

    public EnemyBaseState(EnemyController controller)
    {
        _controller = controller;
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        OnExit();

        newState.OnEnter();

        _controller.CurrentState = newState;
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
        if (_controller.IsStationary)
            return;

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

        Vector3 direction = _controller.Path.vectorPath[currentWaypoint] - _controller.transform.position;
        Vector3 facingDirection = new Vector3(direction.x, 0f, direction.z).normalized;
        direction = direction.normalized;

        Vector3 velocity = direction * _controller.MovementSpeed;

        _controller.transform.forward = facingDirection;
        _controller.Controller.Move(velocity * Time.fixedDeltaTime);
    }

    public virtual void OnHit(DamageInfo damageInfo)
    {
        if (!_target)
            _controller.GetAISensor.SetTarget(damageInfo.source.transform);

        _controller.CurrentHealth = (int)Mathf.Clamp(_controller.CurrentHealth - damageInfo.damage, 0f, _controller.MaxHealth);

        SwitchState(_controller.GetState("Hit"));
    }
}
