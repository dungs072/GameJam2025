using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerConfig
{


    public class MovementSettings
    {
        public const float FRICTION = 100f;
        public const float MOVE_SPEED = 33f;
        public const float RUN_SPEED_MULTIPLIER = 2.5f;
        public const float JUMP_HEIGHT = 16;

        public const float DASH_DURATION = 0.2f;
        public const float DASH_DISTANCE = 7f;
    }

    public class SkinNames
    {
        public const string BLACK = "Black";
        public const string WHITE = "White";
        public const string RED = "Red";
        public const string BLUE = "Blue";
        public const string YELLOW = "Yellow";
        public const string GREEN = "Green";
        public const string PURPLE = "Purple";
        public const string CYAN = "Cyan";

        public static string ProductIDToSkinName(List<string> availableColorIds)
        {
            if (availableColorIds.Contains(ProductID.RED) && availableColorIds.Contains(ProductID.BLUE) && availableColorIds.Contains(ProductID.GREEN))
            {
                return WHITE;
            } else if (availableColorIds.Contains(ProductID.RED) && availableColorIds.Contains(ProductID.BLUE))
            {
                return PURPLE;
            }
            else if (availableColorIds.Contains(ProductID.RED) && availableColorIds.Contains(ProductID.GREEN))
            {
                return YELLOW;
            }
            else if (availableColorIds.Contains(ProductID.BLUE) && availableColorIds.Contains(ProductID.GREEN))
            {
                return CYAN;
            }
            else if (availableColorIds.Contains(ProductID.RED))
            {
                return RED;
            }
            else if (availableColorIds.Contains(ProductID.BLUE))
            {
                return BLUE;
            }
            else if (availableColorIds.Contains(ProductID.GREEN))
            {
                return GREEN;
            }

            return BLACK;
        }
    }

    public class AnimationNames
    {
        public const string IDLE = "Idle";
        public const string WALK = "Walk";
        public const string JUMP_UP = "Jump up";
        public const string JUMP_DOWN = "Jump down";
    }
}
