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

    public void LoadLevel(int levelIndex)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Levels/level" + levelIndex);

        if (jsonFile == null)
        {
            Debug.LogError("Level JSON not found: Levels/level" + levelIndex);
            return;
        }

        CurrentLevel = JsonUtility.FromJson<LevelData>(jsonFile.text);
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
            string typeStr = p.type.ToString();
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
            Vector3 itemPosition = new Vector3(
                itemData.x + World.TileWidth / 2f,
                itemData.y + World.TileHeight / 2f + World.SkewedTileHeight,
                0f
            );
            item.transform.position = itemPosition;
        }
    }
}


