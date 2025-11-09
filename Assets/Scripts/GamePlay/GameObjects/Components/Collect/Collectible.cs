using UnityEngine;

public class Collectible : MonoBehaviour, IPropComponent
{
    [SerializeField] private int amount = 1;
    private ProductData productData;
    private int currentAmount;
    void OnEnable()
    {
        currentAmount = amount;
    }
    void Awake()
    {
        productData = GetComponent<Prop>().productData;
        currentAmount = amount;
    }
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        if (character.IsFullInventory()) return false;
        character.AddItemToInventory(productData.Id, ref currentAmount);
        gameObject.SetActive(currentAmount > 0);
        return true;
    }
}
