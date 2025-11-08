using UnityEngine;

[CreateAssetMenu(fileName = "New Product", menuName = "Factory/Product")]
public class ProductData : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public string DisplayName { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }


}