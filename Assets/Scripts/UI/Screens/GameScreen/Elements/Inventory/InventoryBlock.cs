using UnityEngine;
using UnityEngine.UI;
public class InventoryBlock : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image iconDisabledImage;
    [SerializeField] private string productId;

    [SerializeField] private ProgressBar progressBar;

    void Start()
    {
        SetData(0);
        HideProgressBar();
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

    public void SetProgress(float currentValue, float maxValue)
    {
        if (maxValue == 0)
        {
            HideProgressBar();
            return;
        }
        if (!iconImage.enabled) return;
        ShowProgressBar();
        progressBar.SetProgress(currentValue, maxValue);
        if (currentValue >= maxValue)
        {
            HideProgressBar();
        }
    }

    private void HideProgressBar()
    {
        progressBar.gameObject.SetActive(false);
    }
    private void ShowProgressBar()
    {
        progressBar.gameObject.SetActive(true);
    }
}
