using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Movement movement;
    private InputHandler inputHandler;

    void Awake()
    {
        InitComponents();
        SetUpValues();
    }
    private void InitComponents()
    {

        inputHandler = new InputHandler();
    }
    private void SetUpValues()
    {
        movement.SetUp();
    }
    void Update()
    {
        movement.Update(inputHandler.MoveValue, inputHandler.IsJumping);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        movement.UpdateGroundState(collision);
    }


}
