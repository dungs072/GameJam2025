using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform bottomCameraLimit;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 5f;

    private Camera thisCamera;

    void Awake()
    {
        thisCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        if ((desiredPosition - transform.position).magnitude < MathConfig.CUSTOM_EPSILON)
        {
            transform.position = desiredPosition;
        }

        ConstrainCameraPositionInBounds();
    }

    void ConstrainCameraPositionInBounds()
    {
        var minYPosition = bottomCameraLimit.position.y + thisCamera.orthographicSize;
        if (bottomCameraLimit != null && transform.position.y < minYPosition)
        {
            transform.position = new Vector3(transform.position.x, minYPosition, transform.position.z);
        }
    }
}
