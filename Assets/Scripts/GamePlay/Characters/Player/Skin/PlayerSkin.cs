using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerSkin
{
    [SerializeField] private SpriteRenderer renderer;

    public void SwitchSkinColor(List<string> availableColorIds)
    {
        var colorRuler = GameController.Instance.ColorRuler;
        var mergeColor = colorRuler.FindMergeColor(availableColorIds);
        var colorHex = ColorEnumExtensions.ToHex(mergeColor);
        var color = ColorUtility.TryParseHtmlString(colorHex, out var resultColor) ? resultColor : Color.white;
        renderer.color = color;
    }
}
