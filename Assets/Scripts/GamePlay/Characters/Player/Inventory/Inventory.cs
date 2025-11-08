using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class Inventory
{
    public static event Action<string, int> OnInventoryChanged;
    public IReadOnlyDictionary<string, int> Items => items;
    private Dictionary<string, int> items = new();
    private int maxSize = 100;

    public Inventory(int maxSize = 100)
    {
        this.maxSize = maxSize;
    }
    public int GetItemCount(string productId)
    {
        if (items.ContainsKey(productId))
        {
            return items[productId];
        }
        return 0;
    }

    public bool AddItem(string productId, ref int amount)
    {
        Debug.Log($"<color=#ccd7ac>productId: {productId}</color>");
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
        else
        {
            amount = 0;
        }
        if (items.ContainsKey(productId))
        {
            var quantity = items[productId];

            quantity += totalAmountToAdd;
            items[productId] = quantity;
            NotifyInventoryChanged(productId);
        }
        else
        {
            items.Add(productId, totalAmountToAdd);
            NotifyInventoryChanged(productId);
        }

        return true;
    }

    public bool RemoveItem(string productId, int amount)
    {

        if (items.ContainsKey(productId))
        {
            var quantity = items[productId];
            if (quantity >= amount)
            {
                quantity -= amount;
                if (quantity == 0)
                {
                    items.Remove(productId);
                }
                else
                {
                    items[productId] = quantity;
                }
                NotifyInventoryChanged(productId);
                return true;
            }
        }
        return false;
    }
    public void RemoveUnmatchedLeftItems(string productId)
    {

        foreach (var item in items)
        {
            if (item.Key != productId)
            {
                items.Remove(item.Key);
                NotifyInventoryChanged(item.Key);
                break;
            }
        }
    }
    public bool IsFull()
    {
        return GetTotalItems() >= maxSize;
    }

    private int GetTotalItems()
    {
        int total = 0;
        foreach (var quantity in items.Values)
        {
            total += quantity;
        }
        return total;
    }
    private void NotifyInventoryChanged(string productId)
    {
        int newAmount = GetItemCount(productId);
        Debug.Log($"<color=#78ee15>newAmount: {newAmount}</color>");
        OnInventoryChanged?.Invoke(productId, newAmount);
    }

}
