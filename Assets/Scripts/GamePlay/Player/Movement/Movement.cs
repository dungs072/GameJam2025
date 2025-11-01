using System;
using UnityEngine;

[Serializable]
public class Movement
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody2D rb;
    private bool isGrounded = true;
    private float originalGravityScale;

    public void SetUp()
    {
        originalGravityScale = rb.gravityScale;
    }

    public void Update(Vector2 moveInput, bool isJumping)
    {
        Move(moveInput);
        if (isJumping)
        {
            Jump();
        }
        UpdateDynamicGravity();
    }
    public void Move(Vector2 moveInput)
    {
        rb.linearVelocity = new Vector2(moveInput.x * PlayerConfig.MoveSpeed, rb.linearVelocity.y);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            float vY = Mathf.Sqrt(2 * PlayerConfig.Gravity * PlayerConfig.JumpHeight);

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, vY);
            SetGroundedState(false);
        }
    }
    private void UpdateDynamicGravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = originalGravityScale * PlayerConfig.GravityBoostMultiplier;
        }
        else
        {
            rb.gravityScale = originalGravityScale;
        }
    }

    public void UpdateGroundState(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            if (collision.contacts[0].normal.y > 0.5f)
            {
                SetGroundedState(true);
            }
        }
    }

    private void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }
}
