using System;
using Unity.VisualScripting;
using UnityEngine;



[Serializable]
public class Movement
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform transform;

    private InputHandler inputHandler;
    private bool isGrounded = true;
    private bool isDashing = false;
    private Vector3 velocity = Vector3.zero;

    private float dashTimer = 0f;
    private Vector2 dashDir;


    //! use for optimization gravity check
    private Vector3 prePosition = Vector3.zero;

    public void Init(InputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        UpdateGravity();
    }
    public void Update()
    {
        Move(inputHandler.MoveValue);
        TryUpdateGravity();
        if (inputHandler.IsJumping)
        {
            Jump();
        }
        if (inputHandler.DashLeft)
        {

            Dash(transform.right * -1);
        }
        if (inputHandler.DashRight)
        {
            Dash(transform.right);
        }

        UpdateTransformBaseVelocity();
    }
    private void Move(Vector2 moveInput)
    {
        if (isDashing) return;
        var newVelocityX = moveInput.x * (inputHandler.IsRunning ?
                        PlayerConfig.MovementSettings.RUN_SPEED_MULTIPLIER : 1) *
                        PlayerConfig.MovementSettings.MOVE_SPEED;
        velocity = new Vector3(newVelocityX, velocity.y, 0);
    }
    private void TryUpdateGravity()
    {
        prePosition = transform.position;
        UpdateGravity();
    }
    private void UpdateGravity()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer))
        {
            SetGroundedState(true);
            if (velocity.y <= 0)
            {
                velocity = new Vector3(velocity.x, 0, 0);
            }
        }
        else
        {

            velocity += new Vector3(0, -MathConfig.GRAVITY * Time.deltaTime, 0);
            SetGroundedState(false);
        }
    }


    private void Jump()
    {
        if (!isGrounded) return;
        var newY = Mathf.Sqrt(2 * MathConfig.GRAVITY * PlayerConfig.MovementSettings.JUMP_HEIGHT);
        velocity = new Vector3(velocity.x, newY, 0);
        SetGroundedState(false);
    }
    private void Dash(Vector2 direction)
    {
        inputHandler.ResetFlags();
        if (!isGrounded || isDashing) return;
        isDashing = true;
        dashTimer = 0f;
        dashDir = direction.normalized;

        float dashVelocity = PlayerConfig.MovementSettings.DASH_DISTANCE /
                                PlayerConfig.MovementSettings.DASH_DURATION;
        velocity += new Vector3(dashDir.x * dashVelocity, 0, 0);
        SetGroundedState(false);
    }
    private void UpdateTransformBaseVelocity()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
            velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime /
                        PlayerConfig.MovementSettings.DASH_DURATION);
            if (dashTimer >= PlayerConfig.MovementSettings.DASH_DURATION)
            {
                isDashing = false;
                velocity.x = 0;
            }
        }
        else
        {
            transform.position += velocity * Time.deltaTime;
            velocity.x = Mathf.Lerp(velocity.x, 0, PlayerConfig.MovementSettings.FRICTION * Time.deltaTime);
            if (Mathf.Abs(velocity.x) < Mathf.Epsilon)
            {
                velocity.x = 0;
            }

        }
    }



    private void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }
}
