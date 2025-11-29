using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelBuilder : MonoBehaviour
{
    public static event Action<Vector3> OnPlayerStartPositionReady;
    [SerializeField] private Tilemap platformTileMap;
    [SerializeField] private Tilemap blockTileMap;

    private Dictionary<string, List<Vector3Int>> blockPositions = new();

    private const int BATCH_SIZE = 50;

    void Awake()
    {
        Inventory.OnInventoryItemsChanged += HandleCollectibleColor;
    }
    void OnDestroy()
    {
        Inventory.OnInventoryItemsChanged -= HandleCollectibleColor;
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
    public IEnumerator BuildMapAsync(LevelData level, Action<float> onProgress = null)
    {
        ClearAllTileMaps();
        yield return StartCoroutine(BuildPlatformsCoroutine(level));
        onProgress?.Invoke(0.75f);

        yield return StartCoroutine(BuildBlocksCoroutine(level));
        onProgress?.Invoke(0.9f);
        yield return StartCoroutine(BuildItemsCoroutine(level));
        BuildPlayerStartPosition(level);
    }
    private void ClearAllTileMaps()
    {
        platformTileMap.ClearAllTiles();
        blockTileMap.ClearAllTiles();
        blockPositions.Clear();
    }

    private IEnumerator BuildPlatformsCoroutine(LevelData level)
    {
        var loader = GameController.Instance.Loader;
        int counter = 0;

        foreach (var p in level.platforms)
        {
            string typeStr = p.tileId.ToString();
            var platformTile = loader.LoadedTilesDict[typeStr];

            int startX = p.x;
            int endX = p.x + p.width;
            int startY = -p.y;
            int endY = -p.y - p.height;

            for (int ix = startX; ix < endX; ix++)
            {
                for (int iy = startY; iy > endY; iy--)
                {
                    Vector3Int pos = new Vector3Int(ix, iy, 0);
                    platformTileMap.SetTile(pos, platformTile);

                    if (++counter % BATCH_SIZE == 0)
                    {
                        yield return null;
                    }
                }
            }
        }
    }
    private IEnumerator BuildBlocksCoroutine(LevelData level)
    {
        var loader = GameController.Instance.Loader;
        int counter = 0;
        foreach (var b in level.blocks)
        {
            var blockTile = loader.LoadedTilesDict[b.type];

            int startX = b.x;
            int endX = b.x + b.width;
            int startY = -b.y;
            int endY = -b.y - b.height;

            for (int ix = startX; ix < endX; ix++)
            {
                for (int iy = startY; iy > endY; iy--)
                {
                    Vector3Int pos = new Vector3Int(ix, iy, 0);
                    blockTileMap.SetTile(pos, blockTile);
                    if (!blockPositions.ContainsKey(b.type))
                        blockPositions[b.type] = new List<Vector3Int>();
                    blockPositions[b.type].Add(pos);

                    if (++counter % BATCH_SIZE == 0)
                        yield return null;
                }
            }
        }
    }

    private IEnumerator BuildItemsCoroutine(LevelData level)
    {
        var factory = GameController.Instance.Factory;
        int counter = 0;

        foreach (var itemData in level.items)
        {
            int startY = -itemData.y;
            int newY = startY;
            Vector3Int cellPos = new Vector3Int(itemData.x, newY, 0);
            var newPos = platformTileMap.GetCellCenterWorld(cellPos);
            var item = factory.GetProduct(itemData.type, newPos);
            if (++counter % BATCH_SIZE == 0)
                yield return null;
        }
    }
    public void BuildPlayerStartPosition(LevelData level)
    {
        var newY = level.playerStart.y - 4;
        var startCell = new Vector3Int(level.playerStart.x, newY, 0);
        var worldPos = platformTileMap.GetCellCenterWorld(startCell);
        OnPlayerStartPositionReady?.Invoke(worldPos);
    }
}
