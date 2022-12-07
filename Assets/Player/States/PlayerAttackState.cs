using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerBaseState
{
    private const int MAX_COMBO = 3;

    private int _currentAttack;
    private int _attackPresses;
    private float _attackLockTimer;
    private bool _isInAir;

    private LayerMask hitMasks;

    public PlayerAttackState(PlayerController player) : base(player)
    {
        hitMasks = LayerMask.GetMask("Enemy");
    }

    public override void OnEnter()
    {
        _player._currentVelocity = _player._appliedVelocity = Vector3.zero;

        _attackPresses = 1;
        _player.IsAttackPressed = false;

        _currentAttack = 0;

        DoAttack();
    }

    public override void OnExit()
    {
        _player.IsAttackPressed = false;
    }

    public override void OnUpdate()
    {
        if (_player.IsAttackPressed)
        {
            _attackPresses = Mathf.Clamp(_attackPresses + 1, 0, MAX_COMBO);
            _player.IsAttackPressed = false;
        }

        if (Time.time >= _attackLockTimer && _currentAttack < _attackPresses)
        {
            DoAttack();
        }

        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (Time.time >= _attackLockTimer + 0.3f)
        {
            SwitchState(_player.GetState("Grounded"));
        }
    }

    private void DoAttack()
    {
        _currentAttack = Mathf.Clamp(_currentAttack + 1, 0, MAX_COMBO);

        int attackAnimationHash = Animator.StringToHash("Attack" + _currentAttack);

        _player.Animator.CrossFade(attackAnimationHash, .1f);

        AttackLogic();

        _attackLockTimer = Time.time + .2f;
    }

    private void AttackLogic()
    {
        Collider[] hitColliders = new Collider[20];
        BoxCollider hitboxCollider = _player.GetAttackColliders[_currentAttack - 1];

        Vector3 hitboxPosition = _player.transform.position + hitboxCollider.center;

        int foundColliders = Physics.OverlapBoxNonAlloc(hitboxPosition, hitboxCollider.size / 2, hitColliders, Quaternion.identity, hitMasks);

        Debug.Log(foundColliders);

        if (foundColliders == 0)
        {
            return;
        }

        for (int i = 0; i < foundColliders; i++)
        {
            Debug.Log(hitColliders[i].gameObject.name);

            if (hitColliders[i].TryGetComponent(out IDamageable damageable))
            {
                damageable.OnHit(new DamageInfo(_player.GetComponent<IDamageSource>(), 5));
            }
        }
    }
}