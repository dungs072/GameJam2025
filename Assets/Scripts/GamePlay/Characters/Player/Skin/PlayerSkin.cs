using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

[Serializable]
public class PlayerSkin
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;

    private Movement movement;
    private float walkRemainTime = 0f;

    public void SetPlayerMovement(Movement movement)
    {
        this.movement = movement;
    }

    public void SwitchSkinColor(List<string> availableColorIds)
    {
        skeletonAnimation.Skeleton.SetSkin(PlayerConfig.SkinNames.ProductIDToSkinName(availableColorIds));
    }

    private string CurrentAnimationName => skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name;

    public void Update()
    {
        if (movement.IsGrounded && movement.IsWalking)
        {
            walkRemainTime = PlayerConfig.MovementSettings.WALK_AT_LEAST_DURATION;
        }
        if (walkRemainTime > 0f)
        {
            walkRemainTime -= Time.deltaTime;
            if (walkRemainTime < 0f) walkRemainTime = 0f;
        }

        skeletonAnimation.skeleton.ScaleX = movement.IsLookingRight ? 1 : -1;

        if (movement.IsGrounded && movement.IsWalking && CurrentAnimationName != PlayerConfig.AnimationNames.WALK)
        {
            var entry = skeletonAnimation.AnimationState.SetAnimation(0, PlayerConfig.AnimationNames.WALK, true);
            entry.TimeScale = 2.5f;
        }

        if (movement.IsGrounded && !movement.IsWalking && CurrentAnimationName != PlayerConfig.AnimationNames.IDLE)
        {
            if (walkRemainTime > 0f) return;
            skeletonAnimation.AnimationState.SetAnimation(0, PlayerConfig.AnimationNames.IDLE, true);
        }

        if (!movement.IsGrounded && movement.IsJumpingUp && CurrentAnimationName != PlayerConfig.AnimationNames.JUMP_UP)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, PlayerConfig.AnimationNames.JUMP_UP, false);
        }

        if (!movement.IsGrounded && !movement.IsJumpingUp && CurrentAnimationName != PlayerConfig.AnimationNames.JUMP_DOWN)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, PlayerConfig.AnimationNames.JUMP_DOWN, false);
        }
    }
}
