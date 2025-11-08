using UnityEngine;

public class Collectible : MonoBehaviour, IPropComponent
{
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        return true;
    }
}
