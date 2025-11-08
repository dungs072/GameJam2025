using System.Collections.Generic;
using UnityEngine;

public class Passable : MonoBehaviour, IPropComponent
{
    [SerializeField] private List<ColorStack> ownColors;
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        if (!CanGoThrough(character)) return false;
        foreach (var colorStack in ownColors)
        {
            character.RemoveUnmatchedLeftItems(ColorEnumExtensions.ToID(colorStack.color));
        }
        return true;
    }
    private bool CanGoThrough(ICharacter character)
    {
        foreach (var colorStack in ownColors)
        {
            int countInInventory = character.GetCountItemInventory(ColorEnumExtensions.ToID(colorStack.color));
            if (countInInventory == 0) return false;
        }
        return true;
    }
}
