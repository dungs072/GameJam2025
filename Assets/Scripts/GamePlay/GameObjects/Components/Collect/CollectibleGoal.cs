using UnityEngine;

public class CollectibleGoal : MonoBehaviour, IPropComponent
{
    private ProductData productData;
    void Awake()
    {
        productData = GetComponent<Prop>().productData;
    }
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        GameController.Instance.HandleGameWin();
        return true;
    }
}
