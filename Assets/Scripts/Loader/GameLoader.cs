using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


[Serializable]
public class GameLoader
{

    public static event Action<string, Prop> OnPropPrefabsLoaded;
    public static event Action OnAllPrefabsLoaded;
    [Header("Assign multiple Addressable prefab references here")]
    [SerializeField] private List<AssetReferenceGameObject> prefabReferences;
    [Header("Assign Tile map data")]
    [SerializeField] private List<AssetReference> tileReferences;
    private readonly List<Prop> loadedObjects = new();
    private readonly Dictionary<string, Prop> loadedObjectsDict = new();
    public IReadOnlyDictionary<string, Prop> LoadedObjectsDict => loadedObjectsDict;

    private readonly List<MyTile> loadedTiles = new();
    private readonly Dictionary<string, MyTile> loadedTilesDict = new();
    public IReadOnlyDictionary<string, MyTile> LoadedTilesDict => loadedTilesDict;

    public void LoadAllPrefabs()
    {
        int totalAssets = prefabReferences.Count + tileReferences.Count;
        int loadedCount = 0;
        void CheckAllLoaded()
        {
            loadedCount++;
            if (loadedCount >= totalAssets)
            {
                Debug.Log("All prefabs and tiles loaded!");
                OnAllPrefabsLoaded?.Invoke();
            }
        }
        foreach (var prefabRef in prefabReferences)
        {
            if (prefabRef == null)
            {
                Debug.LogWarning("One of the prefab references is missing!");
                CheckAllLoaded();
                continue;
            }

            prefabRef.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var prefab = handle.Result; // This is the prefab asset, NOT an instance
                    Debug.Log($"Loaded prefab asset: {prefab.name}");
                    if (prefab.TryGetComponent<Prop>(out var propComponent))
                    {
                        loadedObjects.Add(propComponent);
                        loadedObjectsDict[propComponent.productData.Id] = propComponent;
                        OnPropPrefabsLoaded?.Invoke(propComponent.productData.Id, propComponent);
                    }
                }
                CheckAllLoaded();
            };
        }
        foreach (var tileRef in tileReferences)
        {
            if (tileRef == null)
            {
                Debug.LogWarning("One of the tile references is missing!");
                CheckAllLoaded();
                continue;
            }

            tileRef.LoadAssetAsync<MyTile>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var tile = handle.Result; // This is the tile asset, NOT an instance
                    Debug.Log($"Loaded tile asset: {tile.name}");
                    loadedTiles.Add(tile);
                    loadedTilesDict[tile.Id] = tile;
                }

            };
            CheckAllLoaded();
        }
    }

    public void UnloadAllPrefabs()
    {
        foreach (var obj in loadedObjects)
        {
            if (obj != null)
            {
                // Find its reference (if any)
                var refMatch = prefabReferences.Find(r => r.AssetGUID == obj.name);
                if (refMatch != null)
                    refMatch.ReleaseInstance(obj.gameObject);
                else
                    Addressables.ReleaseInstance(obj.gameObject);
            }
        }
        foreach (var tile in loadedTiles)
        {
            if (tile != null)
            {
                var refMatch = tileReferences.Find(r => r.AssetGUID == tile.name);
                if (refMatch != null)
                    Addressables.Release(tile);
                else
                    Addressables.Release(tile);
            }
        }

        loadedObjects.Clear();
        loadedObjectsDict.Clear();
        loadedTiles.Clear();
        loadedTilesDict.Clear();
        Debug.Log("All prefabs unloaded.");
    }
}
