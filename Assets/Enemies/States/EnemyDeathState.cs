using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyBaseState
{
    private float corpseTimer;

    public EnemyDeathState(EnemyController controller) : base (controller) { }

    public override void CheckSwitchStates()
    {
    }

    public override void OnEnter()
    {
        corpseTimer = Time.time;

        _controller.Animator.Play("Death");
        _controller.GetCharacterController.enabled = false;
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
        if (corpseTimer + 10f <= Time.time)
        {
            _controller.GetCharacterController.enabled = true;
            _controller.OnDeath();
        }
    }

    public override void OnHit(DamageInfo damageInfo)
    {
        // Nothing, at least for now.
    }
}
