using System;
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
}
