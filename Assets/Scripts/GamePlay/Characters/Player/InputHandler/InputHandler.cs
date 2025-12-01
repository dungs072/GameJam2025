using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public enum ThrowType
{
    THROW_ONE = 1,
    THROW_TWO = 2,
    THROW_THREE = 3,
}
public class InputHandler : MonoBehaviour
{
    public static event Action<float, float, ThrowType> OnHoldingThrow;
    public event Action<ThrowType> OnThrow;
    public event Action OnRetry;
    private PlayerControls _actions;

    public Vector2 MoveValue { get; private set; }
    public bool IsRunning { get; private set; }
    public bool IsJumping { get; private set; }


    public bool DashLeft { get; private set; }
    public bool DashRight { get; private set; }
    private float _lastTapTimeA = -1f;
    private float _lastTapTimeD = -1f;
    private readonly float _doubleTapThreshold = 0.3f;
    private readonly float _holdThrowThreshold = 0.5f;

    private Coroutine holdThrowCoroutine;

    private void Awake()
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

        // ===== Throw =====
        _actions.Player.Throw1.started += _ => OnStartedThrow(ThrowType.THROW_ONE);
        _actions.Player.Throw1.canceled += _ => OnFireCanceled(ThrowType.THROW_ONE);

        _actions.Player.Throw2.started += _ => OnStartedThrow(ThrowType.THROW_TWO);
        _actions.Player.Throw2.canceled += _ => OnFireCanceled(ThrowType.THROW_TWO);

        _actions.Player.Throw3.started += _ => OnStartedThrow(ThrowType.THROW_THREE);
        _actions.Player.Throw3.canceled += _ => OnFireCanceled(ThrowType.THROW_THREE);

        // ===== Reset =====
        _actions.Player.Retry.performed += _ => OnRetry?.Invoke();
    }
    private void OnDestroy()
    {
        // ===== Movement =====
        _actions.Player.Move.performed -= OnMovePerformed;
        _actions.Player.Move.canceled -= _ => MoveValue = Vector2.zero;

        // ===== Running =====
        _actions.Player.Run.performed -= _ => IsRunning = true;
        _actions.Player.Run.canceled -= _ => IsRunning = false;

        // ===== Jump =====
        _actions.Player.Jump.performed -= _ => IsJumping = true;
        _actions.Player.Jump.canceled -= _ => IsJumping = false;
        // ===== Throw =====
        _actions.Player.Throw1.started -= _ => OnStartedThrow(ThrowType.THROW_ONE);
        _actions.Player.Throw1.canceled -= _ => OnFireCanceled(ThrowType.THROW_ONE);

        _actions.Player.Throw2.started -= _ => OnStartedThrow(ThrowType.THROW_TWO);
        _actions.Player.Throw2.canceled -= _ => OnFireCanceled(ThrowType.THROW_TWO);

        _actions.Player.Throw3.started -= _ => OnStartedThrow(ThrowType.THROW_THREE);
        _actions.Player.Throw3.canceled -= _ => OnFireCanceled(ThrowType.THROW_THREE);
    }

    private void OnStartedThrow(ThrowType throwType)
    {
        holdThrowCoroutine = StartCoroutine(HoldThrowCoroutine(throwType));
    }

    private void OnFireCanceled(ThrowType throwType)
    {
        if (holdThrowCoroutine != null)
        {
            StopCoroutine(holdThrowCoroutine);
            holdThrowCoroutine = null;
            OnHoldingThrow?.Invoke(0f, 0f, throwType);
        }
    }
    private IEnumerator HoldThrowCoroutine(ThrowType throwType)
    {
        float holdTime = 0f;
        while (holdTime < _holdThrowThreshold)
        {
            holdTime += Time.deltaTime;
            OnHoldingThrow?.Invoke(holdTime, _holdThrowThreshold, throwType);
            yield return null;
        }
        OnThrow?.Invoke(throwType);
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
