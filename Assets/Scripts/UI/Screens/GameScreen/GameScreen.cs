using UnityEngine;

public class GameScreen : BaseScreen
{
    [SerializeField] private InventoryUI inventoryUI;

    void Awake()
    {
        Inventory.OnInventoryChanged += UpdateInventoryUI;
    }
    void OnDestroy()
    {
        Inventory.OnInventoryChanged -= UpdateInventoryUI;
    }
    private void UpdateInventoryUI(string productId, int newAmount)
    {
        if (GameController.Instance == null) return;
        var gameLoader = GameController.Instance.Loader;
        Debug.Log($"<color=#562a33>productId: {productId}</color>");
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
