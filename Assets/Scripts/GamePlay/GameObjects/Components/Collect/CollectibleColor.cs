using UnityEngine;

public class CollectibleColor : MonoBehaviour, IPropComponent
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
        var colorRuler = GameController.Instance.ColorRuler;
        var parentColors = colorRuler.GetParentColors(productData.Id);
        Debug.Log($"<color=#c780de>parentColors: {parentColors.Length}</color>");
        if (parentColors.Length == 0)
        {
            character.AddItemToInventory(productData.Id, ref currentAmount);
        }
        else
        {
            foreach (var parentColor in parentColors)
            {
                var newAmount = currentAmount;
                var id = ColorEnumExtensions.ToID(parentColor);
                character.AddItemToInventory(id, ref newAmount);
            }
        }
        //! take all collected items
        gameObject.SetActive(false);
        return true;
    }
}
