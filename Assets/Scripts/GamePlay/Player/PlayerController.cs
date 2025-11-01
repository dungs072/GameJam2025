using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement movement;
    private InputHandler inputHandler;

    void Awake()
    {
        InitComponents();
    }
    private void InitComponents()
    {

        inputHandler = new InputHandler();
    }
    void Start()
    {
        movement.Start();
    }
    void Update()
    {
        movement.Update(inputHandler.MoveValue, inputHandler.IsJumping);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 100f);
    }


}
