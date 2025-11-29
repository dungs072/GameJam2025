using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Tilemaps;
[Serializable]
public class LevelManager
{
    public event Action<LevelData> OnLevelLoaded;
    public LevelData CurrentLevel { get; private set; }


    public void LoadLevelAsync(int level)
    {
        Addressables.InitializeAsync().Completed += _ =>
        {
            Addressables.LoadAssetsAsync<TextAsset>(
                "levels",
                null
            ).Completed += handle => HandleLoadedLevel(handle, level);
        };
    }

    private void HandleLoadedLevel(AsyncOperationHandle<IList<TextAsset>> handle, int level)
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to load level JSON");
            return;
        }

        IList<TextAsset> jsonFiles = handle.Result;
        Debug.Log("Loaded level json");

        if (jsonFiles == null || jsonFiles.Count == 0)
        {
            Debug.LogError("Level JSON not found.");
            return;
        }
        TextAsset jsonFile = jsonFiles[Mathf.Clamp(level, 0, jsonFiles.Count - 1)];
        var levels = JsonUtility.FromJson<LevelsData>(jsonFile.text);
        CurrentLevel = levels.levels[0];
        OnLevelLoaded?.Invoke(CurrentLevel);
    }

}


