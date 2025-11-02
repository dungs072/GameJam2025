using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{

    private PlayerControls _actions;

    public Vector2 MoveValue { get; private set; }

    public bool IsJumping { get; private set; }
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

        // please add your input bindings here

        // Movement
        _actions.Player.Move.performed += ctx => MoveValue = ctx.ReadValue<Vector2>();
        _actions.Player.Move.canceled += _ => MoveValue = Vector2.zero;


        // Jump 
        _actions.Player.Jump.performed += _ => IsJumping = true;
        _actions.Player.Jump.canceled += _ => IsJumping = false;
    }

    public void Dispose()
    {
        _actions.Player.Disable();
    }
}
