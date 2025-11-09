

public enum ColorEnum
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    CYAN,
    PURPLE,
    WHITE,
    NONE
}

public static class ColorEnumExtensions
{
    public static string ToID(this ColorEnum color)
    {
        return color switch
        {
            ColorEnum.RED => ProductID.RED,
            ColorEnum.GREEN => ProductID.GREEN,
            ColorEnum.BLUE => ProductID.BLUE,
            ColorEnum.YELLOW => ProductID.YELLOW,
            ColorEnum.CYAN => ProductID.CYAN,
            ColorEnum.PURPLE => ProductID.PURPLE,
            ColorEnum.WHITE => ProductID.WHITE,
            _ => throw new System.ArgumentOutOfRangeException()
        };
    }
    public static string ToHex(this ColorEnum color)
    {
        return color switch
        {
            ColorEnum.RED => "#FF0000",
            ColorEnum.GREEN => "#00FF00",
            ColorEnum.BLUE => "#0000FF",
            ColorEnum.YELLOW => "#FFFF00",
            ColorEnum.CYAN => "#00FFFF",
            ColorEnum.PURPLE => "#800080",
            ColorEnum.WHITE => "#FFFFFF",
            _ => "#000000"
        };
    }
}