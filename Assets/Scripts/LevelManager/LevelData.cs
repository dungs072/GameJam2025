using System;

[Serializable]
public class LevelData
{
    public int gridWidth;
    public int gridHeight;

    public PlatformData[] platforms;
    public ItemData[] items;
    public BlockData[] blocks;
    public FilterData[] filters;

    public Position playerStart;
}

[Serializable]
public class PlatformData
{
    public int type = 1;
    public int x;
    public int y;
    public int width;
    public int height;
}

[Serializable]
public class ItemData
{
    public string type;
    public int x;
    public int y;
}

[Serializable]
public class BlockData
{
    public int type;
    public int x;
    public int y;
    public int width;
    public int height;
}

[Serializable]
public class FilterData
{
    public int type;
    public int x;
    public int y;
    public int width;
    public int height;
}

[Serializable]
public class Position
{
    public int x;
    public int y;
}
