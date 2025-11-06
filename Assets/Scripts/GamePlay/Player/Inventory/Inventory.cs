using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct ItemData
{
    public string name;
    public int amount;
    // add more for item properties
}
// Each Item has same amount of space in inventory

[Serializable]
public class Inventory
{
    public IReadOnlyDictionary<string, ItemData> Items => items;
    private Dictionary<string, ItemData> items = new();
    private int maxSize = 100;

    public Inventory(int maxSize = 100)
    {
        this.maxSize = maxSize;
    }


    public bool AddItem(string itemName, ref int amount)
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
        if (items.ContainsKey(itemName))
        {
            ItemData existingItem = items[itemName];

            existingItem.amount += totalAmountToAdd;
            items[itemName] = existingItem;
        }
        else
        {
            ItemData newItem = new() { name = itemName, amount = totalAmountToAdd };
            items.Add(itemName, newItem);
        }

        return true;
    }

    public bool RemoveItem(string itemName, int amount)
    {

        if (items.ContainsKey(itemName))
        {
            ItemData existingItem = items[itemName];
            if (existingItem.amount >= amount)
            {
                existingItem.amount -= amount;
                if (existingItem.amount == 0)
                {
                    items.Remove(itemName);
                }
                else
                {
                    items[itemName] = existingItem;
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
