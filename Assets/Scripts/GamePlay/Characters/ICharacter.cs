using UnityEngine;

public interface ICharacter
{
    int GetCountItemInventory(string itemID);
    void RemoveUnmatchedLeftItems(string itemID);

    bool IsFullInventory();

    void AddItemToInventory(string itemID, ref int amount);
}
