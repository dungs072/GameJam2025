using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    int GetCountItemInventory(string itemID);
    void RemoveUnmatchedLeftColorItems(ColorEnum color);

    bool IsFullInventory();

    void AddItemToInventory(string itemID, ref int amount);
    List<string> GetAllItemIDs();
}
