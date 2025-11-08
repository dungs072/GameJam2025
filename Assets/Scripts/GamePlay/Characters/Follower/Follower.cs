using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask groundLayer;
    private Vector3 prevTargetPosition = Vector3.zero;
    private Vector3 preDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        prevTargetPosition = target.position;
        preDirection = target.right;
    }

    private void Update()
    {
        UpdateGravity();
        if (target == null) return;
        var direction = target.right;
        var toFollower = (prevTargetPosition - target.position).normalized;
        var dot = Vector3.Dot(toFollower, direction);
        if (dot < 0)
        {
            direction *= -1;
            preDirection = direction;
        }
        else if (dot > 0)
        {
            direction *= 1;
            preDirection = direction;
        }
        else
        {
            direction = preDirection;
        }
        prevTargetPosition = target.position;
        var targetPosition = new Vector3(target.position.x + direction.x
                                         * FollowerConfig.FOLLOWER_DISTANCE, transform.position.y,
                                          transform.position.z);

        targetPosition += velocity * Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, targetPosition,
        Time.deltaTime * FollowerConfig.MOVE_SPEED);

    }
    private void UpdateGravity()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.5f, groundLayer))
        {
            if (velocity.y <= 0)
            {
                velocity = new Vector3(0, 0, 0);
            }
        }
        else
        {

            velocity += new Vector3(0, -MathConfig.GRAVITY * Time.deltaTime, 0);
        }
    }


}
