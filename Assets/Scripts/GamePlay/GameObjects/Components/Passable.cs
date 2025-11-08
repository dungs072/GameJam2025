using System.Collections.Generic;
using UnityEngine;

public class Passable : MonoBehaviour, IPropComponent
{
    [SerializeField] private int colorId;
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        return true;
    }
}
