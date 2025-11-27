using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[Serializable]
public class PlayerSkin
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    public void SwitchSkinColor(List<string> availableColorIds)
    {
        skeletonAnimation.Skeleton.SetSkin(PlayerConfig.SkinNames.ProductIDToSkinName(availableColorIds));
    }
}
