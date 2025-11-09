using System.Collections.Generic;
using UnityEngine;

public class Filter : MonoBehaviour, IPropComponent
{
    [SerializeField] private List<ColorStack> ownColors;
    public bool HandleInteractWithCharacter(ICharacter character)
    {

        foreach (var colorStack in ownColors)
        {
            character.RemoveUnmatchedLeftColorItems(colorStack.color);
        }
        return true;
    }
}