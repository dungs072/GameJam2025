using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private Movement movement;
    [SerializeField] private Throw throwAction;
    [SerializeField] private PlayerSkin playerSkin;
    private InputHandler inputHandler;
    private Inventory inventory;
    void Awake()
    {
        InitComponents();
        inputHandler.OnThrow += throwAction.HandleThrow;
    }
    private void InitComponents()
    {
        inputHandler = GetComponent<InputHandler>();
        inventory = new Inventory();
    }
    void Start()
    {
        movement.Init(inputHandler);
        throwAction.Init(inventory, playerSkin, movement);
        playerSkin.SwitchSkinColor(inventory.GetAllItemIDs());
        playerSkin.SetPlayerMovement(movement);
    }
    void OnDestroy()
    {
        inputHandler.OnThrow -= throwAction.HandleThrow;
    }
    void Update()
    {
        movement.Update();
        playerSkin.Update();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 100f);
    }

    public int GetCountItemInventory(string itemID)
    {
        return inventory.GetItemCount(itemID);
    }

    public void RemoveUnmatchedLeftColorItems(ColorEnum color)
    {
        var colorRuler = GameController.Instance.ColorRuler;
        var parentColors = colorRuler.GetParentColors(color);
        var colorIds = new List<string> { ColorEnumExtensions.ToID(color) };
        foreach (var parentColor in parentColors)
        {
            colorIds.Add(ColorEnumExtensions.ToID(parentColor));
        }
        Debug.Log($"<color=#e468d9>colorIds: {colorIds.Count}</color>");
        inventory.RemoveUnMatchedLeftItems(colorIds);
        var availableColorIds = inventory.GetAllItemIDs();
        playerSkin.SwitchSkinColor(availableColorIds);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IPropComponent>(out var propComponent))
        {
            var result = propComponent.HandleInteractWithCharacter(this);

            if (result) return;
            HandleBlockPlayer(collision);
        }
    }
    private void HandleBlockPlayer(Collider2D collision)
    {
        Vector2 facing = transform.right;
        Vector2 toTarget = (collision.transform.position - transform.position).normalized;
        float dot = Vector2.Dot(facing, toTarget);
        movement.SetBlockState(dot < 0 ? BlockState.Left : BlockState.Right);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        movement.SetBlockState(BlockState.None);
    }

    public bool IsFullInventory()
    {
        return inventory.IsFull();
    }

    public void AddItemToInventory(string itemID, ref int amount)
    {
        inventory.AddItem(itemID, ref amount);
        var availableColorIds = inventory.GetAllItemIDs();
        playerSkin.SwitchSkinColor(availableColorIds);
    }

    public List<string> GetAllItemIDs()
    {
        return inventory.GetAllItemIDs();
    }
}
