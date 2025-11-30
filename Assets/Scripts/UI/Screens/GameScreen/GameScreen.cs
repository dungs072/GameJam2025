using BaseEngine;
using UnityEngine;

public class GameScreen : BaseScreen
{
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private MagicButton playAgainButton;

    void Awake()
    {
        Inventory.OnInventoryChanged += UpdateInventoryUI;
        playAgainButton.AddListener(OnPlayAgainButtonClicked);
    }
    void OnDestroy()
    {
        Inventory.OnInventoryChanged -= UpdateInventoryUI;
        playAgainButton.RemoveListener(OnPlayAgainButtonClicked);
    }
    private void OnPlayAgainButtonClicked()
    {
        GameController.Instance.HandlePlayGameAgainWhenNotWin();
    }
    private void UpdateInventoryUI(string productId, int newAmount)
    {
        if (GameController.Instance == null) return;
        var gameLoader = GameController.Instance.Loader;
        if (gameLoader.LoadedObjectsDict.TryGetValue(productId, out var prop))
        {
            inventoryUI.AddItem(productId, prop.productData.Icon, newAmount);
        }
        else
        {
            Debug.LogWarning($"ProductId {productId} not found in LoadedObjectsDict.");
        }
    }
}
