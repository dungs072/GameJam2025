using UnityEngine;
using UnityEngine.UI;
public class InventoryBlock : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image iconDisabledImage;
    [SerializeField] private string productId;

    void Start()
    {
        SetData(0);
    }

    public void SetData(int amount)
    {
        iconDisabledImage.enabled = amount == 0;
        iconImage.enabled = amount > 0;
    }
    public bool HasProductId(string id)
    {
        return productId == id;
    }
}
