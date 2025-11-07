using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private Movement movement;
    private InputHandler inputHandler;
    private Inventory inventory;
    void Awake()
    {
        InitComponents();
    }
    private void InitComponents()
    {
        inputHandler = new InputHandler();
        inventory = new Inventory();
    }
    void Start()
    {
        movement.Init(inputHandler);
    }
    void Update()
    {
        movement.Update();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 100f);
    }


}
