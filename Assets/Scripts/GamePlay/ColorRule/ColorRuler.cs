using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

[Serializable]
public struct ColorData
{
    public ColorEnum[] parents;
    public ColorEnum color;
}


[CreateAssetMenu(fileName = "ColorRuler", menuName = "Game/ColorRuler")]
public class ColorRuler : ScriptableObject
{
    [SerializeField] private ColorData[] colors;

    public ReadOnlyCollection<ColorData> Colors => Array.AsReadOnly(colors);

    public ColorEnum[] GetParentColors(ColorEnum color)
    {
        foreach (var colorData in colors)
        {
            if (colorData.color == color)
            {
                return colorData.parents;
            }
        }
        return Array.Empty<ColorEnum>();
    }
}
