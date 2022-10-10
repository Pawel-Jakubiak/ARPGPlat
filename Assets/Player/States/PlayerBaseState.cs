using UnityEngine;

public abstract class PlayerBaseState : BasicState
{
    protected PlayerController _player;

    public float redSpeed = 1;

    public PlayerBaseState(PlayerController player)
    {
        _player = player;
    }

    protected void SwitchState(PlayerBaseState newState) 
    {
        OnExit();
        newState.OnEnter();

        _player.CurrentState = newState;
    }

    protected virtual void HandleMovement()
    {
        Vector2 newVelocity = _player.CurrentMovementInput * (_player.MovementSpeed * _player.CurrentState.redSpeed);
        _player._currentVelocity.x = newVelocity.x;
        _player._currentVelocity.z = newVelocity.y;

        _player._appliedVelocity.x = newVelocity.x;
        _player._appliedVelocity.z = newVelocity.y;

        if (_player.IsMoving)
            _player.transform.forward = new Vector3(_player.CurrentMovementInput.x, 0f, _player.CurrentMovementInput.y);
    }
}
