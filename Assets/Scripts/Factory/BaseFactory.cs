using System;
using System.Collections.Generic;
using UnityEngine;


public class BaseFactory : MonoBehaviour
{
    [SerializeField] private List<GameObject> products;
    [SerializeField] private int startId = 8;
    private Dictionary<int, GameObject> productRecord = new();
    private Dictionary<int, List<GameObject>> productCache = new();
    private void Awake()
    {
        InitializeProductDictionary();
    }

    private void InitializeProductDictionary()
    {
        productRecord = new Dictionary<int, GameObject>();
        for (int i = 0; i < products.Count; i++)
        {
            productRecord.Add(i, products[i]);
        }
    }

    public void RegisterProduct(GameObject product)
    {
        int id = productRecord.Count;
        if (!productRecord.ContainsKey(id))
        {
            productRecord.Add(id, product);
        }
        else
        {
            Debug.LogWarning($"Product with Name {product.name} is already registered.");
        }
    }
    public GameObject GetProduct(int productId)
    {
        if (!IsValidProductId(productId))
        {
            Debug.LogError($"Product with ID {productId} does not exist.");
            return null;
        }
        var list = productCache.GetValueOrDefault(productId, new List<GameObject>());
        GameObject selectedProduct;
        if (list.Count == 0)
        {
            selectedProduct = CreateNewProduct(productId);
        }
        else
        {
            var unusedProduct = list.Find(p => !p.activeInHierarchy);
            if (unusedProduct == null)
            {
                selectedProduct = CreateNewProduct(productId);
            }
            else
            {
                selectedProduct = unusedProduct;
            }
        }
        selectedProduct.SetActive(true);
        return selectedProduct;
    }
    private bool IsValidProductId(int productId)
    {
        return productRecord.ContainsKey(productId);
    }
    private GameObject CreateNewProduct(int productId)
    {
        GameObject newProduct = Instantiate(productRecord[productId]);
        var list = productCache.GetValueOrDefault(productId, new List<GameObject>());
        list.Add(newProduct);
        productCache[productId] = list;
        return newProduct;
    }
}