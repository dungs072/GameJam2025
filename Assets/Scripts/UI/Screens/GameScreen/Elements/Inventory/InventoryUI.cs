using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private List<InventoryBlock> inventoryBlocks;

    void Awake()
    {
        InputHandler.OnHoldingThrow += UpdateThrowProgress;
    }

    void OnDestroy()
    {
        InputHandler.OnHoldingThrow -= UpdateThrowProgress;
    }

    public void AddItem(string productId, Sprite icon, int amount)
    {
        foreach (var block in inventoryBlocks)
        {
            if (block.HasProductId(productId))
            {
                block.SetData(amount);
            }
        }
    }
    private void UpdateThrowProgress(float holdTime, float holdThreshold, ThrowType throwType)
    {
        if (throwType == ThrowType.THROW_ONE)
        {
            inventoryBlocks[0].SetProgress(holdTime, holdThreshold);
        }
        else if (throwType == ThrowType.THROW_TWO)
        {
            inventoryBlocks[1].SetProgress(holdTime, holdThreshold);
        }
        else if (throwType == ThrowType.THROW_THREE)
        {
            inventoryBlocks[2].SetProgress(holdTime, holdThreshold);
        }
    }





}
