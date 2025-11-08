

public enum ColorEnum
{
    RED,
    GREEN,
    BLUE,
    YELLOW,
    CYAN,
    PURPLE,
    WHITE
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
}