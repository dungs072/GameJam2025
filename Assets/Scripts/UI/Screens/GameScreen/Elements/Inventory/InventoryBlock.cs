using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryBlock : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text amountText;
    private string productId;

    void Start()
    {
        SetData("", null, 0);
    }

    public void SetData(string id, Sprite icon, int amount)
    {
        productId = id;
        iconImage.sprite = amount == 0 ? null : icon;
        string text = amount == 0 ? "" : "X" + amount.ToString();
        amountText.text = text;
        this.iconImage.enabled = iconImage.sprite != null;
    }
    public bool IsEmpty()
    {
        return iconImage.sprite == null;
    }
    public bool HasProductId(string id)
    {
        return productId == id;
    }


}
