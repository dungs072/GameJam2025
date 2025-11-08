using System;

public class ProductStack
{
    public string productId;
    public int quantity;
}
// if quantity = -1 => infinite amount
[Serializable]
public class ColorStack
{
    public ColorEnum color;
    public int quantity = -1;
}