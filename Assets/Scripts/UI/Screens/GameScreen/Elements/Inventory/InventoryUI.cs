using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<InventoryBlock> inventoryBlocks;


    public void AddItem(string productId, Sprite icon, int amount)
    {
        Debug.Log($"<color=#5ee8a5>productId: {productId}</color>");
        Debug.Log($"<color=#b45781>icon: {icon}</color>");
        Debug.Log($"<color=#b720d5>amount: {amount}</color>");
        foreach (var block in inventoryBlocks)
        {
            if (block.HasProductId(productId))
            {
                block.SetData(amount);
            }
        }
    }
}
