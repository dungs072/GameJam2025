using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class InventoryBlock : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image iconDisabledImage;
    [SerializeField] private string productId;

    [SerializeField] private ProgressBar progressBar;
    private Tween shakeTween;
    private Vector2? initialPos;

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
        if (GameController.Instance.IsPlayerInBlockTileMap())
        {
            Shake();
            HideProgressBar();
            return;
        }
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
    private void Shake()
    {
        if (initialPos == null)
        {
            initialPos = GetComponent<RectTransform>().anchoredPosition;
        }
        if (!initialPos.HasValue) return;
        shakeTween?.Kill();
        var target = GetComponent<RectTransform>();
        target.anchoredPosition = initialPos.Value;
        shakeTween = target.DOShakeAnchorPos(
            duration: 0.4f,
            strength: new Vector2(20f, 0f),
            vibrato: 15,
            randomness: 0,
            snapping: false,
            fadeOut: true
        );
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
