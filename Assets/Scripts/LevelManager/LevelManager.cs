using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
[Serializable]
public class LevelManager
{
    [SerializeField] private Tilemap platformTileMap;
    [SerializeField] private Tilemap blockTileMap;
    [SerializeField] private Tilemap filterTileMap;

    private Dictionary<string, List<Vector3Int>> blockPositions = new();

    private TileBase[] filterTiles;

    public LevelData CurrentLevel { get; private set; }

    public LevelManager()
    {
        CollectibleColor.OnColorCollected += HandleCollectibleColor;
    }
    ~LevelManager()
    {
        CollectibleColor.OnColorCollected -= HandleCollectibleColor;
    }
    private void HandleCollectibleColor(List<string> colorIds)
    {
        if (!blockTileMap) return;
        foreach (var kvp in blockPositions)
        {
            var blockType = kvp.Key;
            var block = kvp.Value;
            var isSameColor = colorIds.Contains(blockType);
            foreach (var pos in block)
            {
                var colliderType = Tile.ColliderType.Sprite;
                var color = blockTileMap.GetColor(pos);
                var alpha = 1f;
                if (isSameColor)
                {
                    colliderType = Tile.ColliderType.None;
                    alpha = 0.25f;
                }
                blockTileMap.SetTileFlags(pos, TileFlags.None);
                blockTileMap.SetColliderType(pos, colliderType);
                blockTileMap.SetColor(pos, new Color(color.r, color.g, color.b, alpha));

            }
        }
    }

    public void LoadLevel(int levelIndex)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/level" + levelIndex);

        if (jsonFile == null)
        {
            Debug.LogError("Level JSON not found: Levels/level" + levelIndex);
            return;
        }

        var levels = JsonUtility.FromJson<LevelsData>(jsonFile.text);
        CurrentLevel = levels.levels[levelIndex - 1];
        Debug.Log("Level loaded: level " + levelIndex);

        ClearAllTileMaps();
        BuildPlatforms();
        BuildBlocks();
        BuildItems();
        // BuildFilters();
    }

    private void ClearAllTileMaps()
    {
        platformTileMap.ClearAllTiles();
        blockTileMap.ClearAllTiles();
        blockPositions.Clear();
        // filterTileMap.ClearAllTiles();
        // itemTileMap.ClearAllTiles();
    }

    private void BuildPlatforms()
    {
        var loader = GameController.Instance.Loader;
        foreach (var p in CurrentLevel.platforms)
        {
            string typeStr = p.tileId.ToString();
            var platformTile = loader.LoadedTilesDict[typeStr];
            FillRect(platformTileMap, platformTile, p.x, p.y, p.width, p.height);
        }
    }

    private void BuildBlocks()
    {
        var loader = GameController.Instance.Loader;
        foreach (var b in CurrentLevel.blocks)
        {
            var blockTile = loader.LoadedTilesDict[b.type];
            FillRect(blockTileMap, blockTile, b.x, b.y, b.width, b.height, OnSetTile: (pos) =>
            {
                if (!blockPositions.ContainsKey(b.type))
                {
                    blockPositions[b.type] = new List<Vector3Int>();
                }
                blockPositions[b.type].Add(pos);

            });
        }
    }

    private void BuildFilters()
    {
        foreach (var f in CurrentLevel.filters)
        {
            var tile = filterTiles[f.type];
            FillRect(filterTileMap, tile, f.x, f.y, f.width, f.height);
        }
    }


    private void FillRect(Tilemap tm, TileBase tile, int x, int y, int w, int h, Action<Vector3Int> OnSetTile = null)
    {
        y += World.SkewedTileHeight;
        //? coordinate system is -y: up, +y: down
        y *= -1;
        h *= -1;
        for (int ix = x; ix < x + w; ix++)
        {
            for (int iy = y; iy > y + h; iy--)
            {
                Vector3Int pos = new Vector3Int(ix, iy, 0);
                tm.SetTile(pos, tile);
                OnSetTile?.Invoke(pos);
            }
        }
    }

    private void BuildItems()
    {
        var factory = GameController.Instance.Factory;
        foreach (var itemData in CurrentLevel.items)
        {
            var item = factory.GetProduct(itemData.type);
            var newY = itemData.y - World.SkewedTileHeight;
            Debug.Log($"<color=#2db2fc>newY: {newY}</color>");
            var worldPos = platformTileMap.CellToWorld(new Vector3Int(itemData.x, newY, 0));
            item.transform.position = worldPos;
        }
    }
}


