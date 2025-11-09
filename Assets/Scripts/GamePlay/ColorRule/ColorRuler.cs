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
    public ColorEnum[] GetParentColors(string colorID)
    {
        foreach (var colorData in colors)
        {
            if (ColorEnumExtensions.ToID(colorData.color) == colorID)
            {
                return colorData.parents;
            }
        }
        return Array.Empty<ColorEnum>();
    }
    public ColorEnum FindMergeColor(List<string> inputColors)
    {
        if (inputColors.Count == 0) return ColorEnum.NONE;
        if (inputColors.Count == 1) return GetColorEnumFromID(inputColors[0]);
        foreach (var colorData in colors)
        {
            var parents = colorData.parents;
            bool allMatch = true;
            foreach (var parent in parents)
            {
                var parentID = ColorEnumExtensions.ToID(parent);
                if (!inputColors.Contains(parentID))
                {
                    allMatch = false;
                    break;
                }
            }
            if (allMatch && parents.Length == inputColors.Count)
            {
                return colorData.color;
            }
        }
        return ColorEnum.NONE;
    }
    private ColorEnum GetColorEnumFromID(string colorID)
    {
        foreach (ColorEnum color in Enum.GetValues(typeof(ColorEnum)))
        {
            if (ColorEnumExtensions.ToID(color) == colorID)
            {
                return color;
            }
        }
        return ColorEnum.NONE;
    }
}
