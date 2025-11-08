using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


[Serializable]
public class GameLoader
{
    [Header("Assign multiple Addressable prefab references here")]
    [SerializeField] private List<AssetReferenceGameObject> prefabReferences;
    private readonly List<GameObject> loadedObjects = new();

    public void LoadAllPrefabs()
    {
        foreach (var prefabRef in prefabReferences)
        {
            if (prefabRef == null)
            {
                Debug.LogWarning("One of the prefab references is missing!");
                continue;
            }

            prefabRef.LoadAssetAsync<GameObject>().Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    var prefab = handle.Result; // This is the prefab asset, NOT an instance
                    Debug.Log($"Loaded prefab asset: {prefab.name}");
                }
            };

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
                    refMatch.ReleaseInstance(obj);
                else
                    Addressables.ReleaseInstance(obj);
            }
        }

        loadedObjects.Clear();
        Debug.Log("All prefabs unloaded.");
    }
}
