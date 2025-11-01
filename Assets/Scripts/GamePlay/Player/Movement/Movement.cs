using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Movement
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform transform;
    private bool isGrounded = true;
    private Vector3 velocity = Vector3.zero;

    private Vector3 prePosition = Vector3.zero;

    public void Start()
    {
        UpdateGravity();
    }
    public void Update(Vector2 moveInput, bool isJumping)
    {
        Move(moveInput);
        TryUpdateGravity();
        if (isJumping)
        {
            Jump();
        }
        UpdateTransformBaseVelocity();
    }
    private void Move(Vector2 moveInput)
    {
        velocity = new Vector3(moveInput.x * PlayerConfig.MOVE_SPEED, velocity.y, 0);
    }


    private void Jump()
    {
        if (!isGrounded) return;
        var newY = Mathf.Sqrt(2 * MathConfig.GRAVITY * PlayerConfig.JUMP_HEIGHT);
        velocity = new Vector3(velocity.x, newY, 0);
        SetGroundedState(false);
    }
    private void UpdateTransformBaseVelocity()
    {
        transform.position += velocity * Time.deltaTime;
        velocity.x = Mathf.Lerp(velocity.x, 0, PlayerConfig.FRICTION * Time.deltaTime);

        if (velocity.x < Mathf.Epsilon)
        {
            velocity.x = 0;
        }
    }
    private void TryUpdateGravity()
    {
        prePosition = transform.position;
        UpdateGravity();
    }
    private void UpdateGravity()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.75f, groundLayer))
        {
            SetGroundedState(true);
            if (velocity.y <= 0)
            {
                velocity = new Vector3(velocity.x, 0, 0);
            }
            Debug.Log("Grounded");
        }
        else
        {

            velocity += new Vector3(0, -MathConfig.GRAVITY * Time.deltaTime, 0);
            SetGroundedState(false);
        }
    }


    private void SetGroundedState(bool grounded)
    {
        isGrounded = grounded;
    }
}
