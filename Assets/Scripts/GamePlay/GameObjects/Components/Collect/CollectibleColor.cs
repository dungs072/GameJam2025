using System;
using System.Collections.Generic;
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

    void Update()
    {
        if (PlayerController.Instance == null) return;
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            transform.position += 10f * Time.deltaTime * (PlayerController.Instance.transform.position - transform.position).normalized;
        }
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
        gameObject.SetActive(false);
        return true;
    }
}
