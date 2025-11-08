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
            character.RemoveUnmatchedLeftColorItems(colorStack.color);
        }
        return true;
    }
    private bool CanGoThrough(ICharacter character)
    {
        foreach (var colorStack in ownColors)
        {
            var colorId = ColorEnumExtensions.ToID(colorStack.color);
            int countInInventory = character.GetCountItemInventory(colorId);
            var colorRuler = GameController.Instance.ColorRuler;
            var parentColors = colorRuler.GetParentColors(colorStack.color);
            foreach (var parentColor in parentColors)
            {
                var parentColorId = ColorEnumExtensions.ToID(parentColor);
                countInInventory += character.GetCountItemInventory(parentColorId);
            }
            if (countInInventory == 0) return false;
        }
        return true;
    }
}
