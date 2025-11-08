using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{
    private PlayerControls _actions;

    public Vector2 MoveValue { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }


    public bool DashLeft { get; private set; }
    public bool DashRight { get; private set; }
    private float _lastTapTimeA = -1f;
    private float _lastTapTimeD = -1f;
    private readonly float _doubleTapThreshold = 0.3f;

    public InputHandler()
    {
        _actions = new PlayerControls();
        _actions.Player.Enable();

        GameController.OnInputStateChanged += isEnabled =>
        {
            if (isEnabled)
                _actions.Player.Enable();
            else
                _actions.Player.Disable();
        };

        // ===== Movement =====
        _actions.Player.Move.performed += OnMovePerformed;
        _actions.Player.Move.canceled += _ => MoveValue = Vector2.zero;

        // ===== Running =====
        _actions.Player.Run.performed += _ => IsRunning = true;
        _actions.Player.Run.canceled += _ => IsRunning = false;

        // ===== Jump =====
        _actions.Player.Jump.performed += _ => IsJumping = true;
        _actions.Player.Jump.canceled += _ => IsJumping = false;

    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        MoveValue = ctx.ReadValue<Vector2>();

        if (MoveValue.x > 0.1f)
        {
            if (Time.time - _lastTapTimeD < _doubleTapThreshold)
                DashRight = true;

            _lastTapTimeD = Time.time;
        }
        else if (MoveValue.x < -0.1f)
        {
            if (Time.time - _lastTapTimeA < _doubleTapThreshold)
                DashLeft = true;

            _lastTapTimeA = Time.time;
        }
    }

    public void ResetFlags()
    {
        DashLeft = false;
        DashRight = false;
    }

    public void Dispose()
    {
        _actions.Player.Disable();
    }
}
