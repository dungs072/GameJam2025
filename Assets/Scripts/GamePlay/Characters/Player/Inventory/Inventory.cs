using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemData
{
    public int id;
    public int amount;
    // add more for item properties
}
// Each Item has same amount of space in inventory

[Serializable]
public class Inventory
{
    public IReadOnlyDictionary<int, ItemData> Items => items;
    private Dictionary<int, ItemData> items = new();
    private int maxSize = 100;

    public Inventory(int maxSize = 100)
    {
        this.maxSize = maxSize;
    }


    public bool AddItem(int itemId, ref int amount)
    {
        int totalItem = GetTotalItems();
        if (totalItem >= maxSize)
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }
        int spaceLeft = maxSize - totalItem;
        int totalAmountToAdd = amount;
        if (amount > spaceLeft)
        {
            totalAmountToAdd = spaceLeft;
            amount -= spaceLeft;
        }
        if (items.ContainsKey(itemId))
        {
            ItemData existingItem = items[itemId];

            existingItem.amount += totalAmountToAdd;
            items[itemId] = existingItem;
        }
        else
        {
            ItemData newItem = new() { id = itemId, amount = totalAmountToAdd };
            items.Add(itemId, newItem);
        }

        return true;
    }

    public bool RemoveItem(int itemId, int amount)
    {

        if (items.ContainsKey(itemId))
        {
            ItemData existingItem = items[itemId];
            if (existingItem.amount >= amount)
            {
                existingItem.amount -= amount;
                if (existingItem.amount == 0)
                {
                    items.Remove(itemId);
                }
                else
                {
                    items[itemId] = existingItem;
                }
                return true;
            }
        }
        return false;
    }

    private int GetTotalItems()
    {
        int total = 0;
        foreach (var item in items.Values)
        {
            total += item.amount;
        }
        return total;
    }

}
