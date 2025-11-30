using System;
using UnityEngine;
[Serializable]
public class Throw
{
    [SerializeField] private Transform leftThrowPoint;
    [SerializeField] private Transform rightThrowPoint;
    private Inventory inventory;
    private PlayerSkin playerSkin;
    private Movement movement;

    public void Init(Inventory inventory, PlayerSkin playerSkin, Movement movement)
    {
        this.inventory = inventory;
        this.playerSkin = playerSkin;
        this.movement = movement;
    }

    public void HandleThrow(ThrowType throwType)
    {
        var gameController = GameController.Instance;
        if (gameController.IsPlayerInBlockTileMap())
        {
            return;
        }
        string itemID = throwType switch
        {
            ThrowType.THROW_ONE => "red",
            ThrowType.THROW_TWO => "green",
            ThrowType.THROW_THREE => "blue",
            _ => null
        };
        if (itemID == null) return;
        int amount = inventory.GetItemCount(itemID);
        if (amount == 0) return;
        inventory.RemoveItem(itemID, amount);
        var factory = GameController.Instance.Factory;
        var newPosition = movement.IsLookingRight ? rightThrowPoint.position : leftThrowPoint.position;

        factory.GetProduct(itemID, newPosition);
        playerSkin.SwitchSkinColor(inventory.GetAllItemIDs());
    }
    public Vector2 GetLeftThrowPointPosition()
    {
        return leftThrowPoint.position;
    }
    public Vector2 GetRightThrowPointPosition()
    {
        return rightThrowPoint.position;
    }
}
