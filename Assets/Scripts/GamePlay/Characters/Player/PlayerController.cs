using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, ICharacter
{
    [SerializeField] private Movement movement;
    [SerializeField] private PlayerSkin playerSkin;
    private InputHandler inputHandler;
    private Inventory inventory;
    void Awake()
    {
        InitComponents();
        inventory.OnItemRemoved += HandleItemRemoved;
    }
    private void InitComponents()
    {
        inputHandler = new InputHandler();
        inventory = new Inventory();
    }
    void Start()
    {
        movement.Init(inputHandler);
        playerSkin.SwitchSkinColor(inventory.GetAllItemIDs());
    }
    void OnDestroy()
    {
        inventory.OnItemRemoved -= HandleItemRemoved;
    }
    private void HandleItemRemoved(string itemID, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var factory = GameController.Instance.Factory;
            var item = factory.GetProduct(itemID);
            item.transform.position = transform.position + Vector3.up * 5f;
        }

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
            Debug.Log($"<color=#9a66e4>result: {result}</color>");
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
}
