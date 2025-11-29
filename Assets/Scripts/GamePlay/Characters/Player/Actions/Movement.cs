using System;
using UnityEngine;

public enum BlockState
{
    None,
    Left,
    Right
}

[Serializable]
public class Movement
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform transform;
    [SerializeField] private Collider2D playerCollider;

    private InputHandler inputHandler;
    private bool isGrounded = true;
    private bool isDashing = false;
    private Vector3 velocity = Vector3.zero;

    private float dashTimer = 0f;
    private Vector2 dashDir;
    private BlockState blockState = BlockState.None;

    private bool isLookingRight = true;

    // For animation
    public bool IsGrounded => isGrounded;
    public bool IsJumpingUp => velocity.y > 0;
    public bool IsLookingRight => isLookingRight;
    public bool IsWalking => velocity.x != 0;

    


    //! use for optimization gravity check
    private Vector3 prePosition = Vector3.zero;

    public void SetBlockState(BlockState state)
    {
        blockState = state;
    }

    public void Init(InputHandler inputHandler)
    {
        this.inputHandler = inputHandler;
        UpdateGravity();
    }
    public void Update()
    {
        Move(inputHandler.MoveValue);
        TryUpdateGravity();
        UpdateBlockMove();
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
    public void UpdateBlockMove()
    {
        var isLeft = CheckWall(Vector2.left);
        var isRight = CheckWall(Vector2.right);
        SetBlockState(isLeft ? BlockState.Left : isRight ? BlockState.Right : BlockState.None);
    }
    bool CheckWall(Vector2 direction)
    {
        if (direction == Vector2.left && isLookingRight) return false;
        if (direction == Vector2.right && !isLookingRight) return false;
        if (direction == Vector2.left)
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                playerCollider.bounds.center + playerCollider.bounds.extents.x * Vector3.left + Vector3.up * 0.1f + Vector3.right * 0.5f,
                new Vector3(0.2f, playerCollider.bounds.size.y - 0.2f, 0),
                0f,
                direction,
                0.4f,
                groundLayer
            );
            return hit.collider != null;
        } else if (direction == Vector2.right)
        {
            RaycastHit2D hit = Physics2D.BoxCast(
                playerCollider.bounds.center + playerCollider.bounds.extents.x * Vector3.right + Vector3.up * 0.1f + Vector3.left * 0.5f,
                new Vector3(0.2f, playerCollider.bounds.size.y - 0.2f, 0),
                0f,
                direction,
                0.4f,
                groundLayer
            );
            return hit.collider != null;
        }

        return false;
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
        var raycastHit = Physics2D.Raycast(playerCollider.bounds.center + Vector3.down * (playerCollider.bounds.extents.y - 0.4f), Vector2.down, 0.42f, groundLayer);
        if (raycastHit.collider != null)
        {
            SnapToGround(raycastHit);
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
    private void SnapToGround(RaycastHit2D hit)
    {
        transform.position = new Vector3(transform.position.x, hit.point.y + playerCollider.bounds.extents.y - playerCollider.offset.y * transform.localScale.y + 0.01f, transform.position.z);
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
        Vector3 newPosition = transform.position;


        if (blockState == BlockState.Left && velocity.x < 0)
        {
            velocity.x = 0;
        }
        else if (blockState == BlockState.Right && velocity.x > 0)
        {
            velocity.x = 0;
        }

        if (isDashing)
        {
            dashTimer += Time.deltaTime;
            newPosition = transform.position + velocity * Time.deltaTime;

            if (velocity.x > 0) isLookingRight = true;
            else if (velocity.x < 0) isLookingRight = false;

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
            newPosition += velocity * Time.deltaTime;

            if (velocity.x > 0) isLookingRight = true;
            else if (velocity.x < 0) isLookingRight = false;

            velocity.x = Mathf.Lerp(velocity.x, 0, PlayerConfig.MovementSettings.FRICTION * Time.deltaTime);
            if (Mathf.Abs(velocity.x) < Mathf.Epsilon)
            {
                velocity.x = 0;
            }

        }

        transform.position = newPosition;
    }



    private void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }
}
