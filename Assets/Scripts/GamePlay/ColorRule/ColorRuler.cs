using System;
using UnityEngine;

[Serializable]
public struct ColorData
{
    public int[] parents;

    public string name;
    public int id;
}


[CreateAssetMenu(fileName = "ColorRuler", menuName = "Game/ColorRuler")]
public class ColorRuler : ScriptableObject
{
    [SerializeField] private ColorData[] colors;
    public int ColorCount => colors?.Length ?? 0;
}
