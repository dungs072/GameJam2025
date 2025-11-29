using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public static event Action<float> OnBootGame;
    [field: SerializeField] public BaseFactory Factory { get; private set; }
    [field: SerializeField] public GameLoader Loader { get; private set; }
    [Header("Data")]
    [field: SerializeField] public ColorRuler ColorRuler { get; private set; }
    [Header("Level manager")]
    [field: SerializeField] public LevelManager LevelManager { get; private set; } = new();
    [SerializeField] private LevelBuilder levelBuilder;
    [Header("Player")]
    [SerializeField] private PlayerController playerController;
    [Header("Input")]
    [SerializeField] private EventSystem eventSystem;
    public static event Action<bool> OnInputStateChanged;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Initialize();
        GameLoader.OnPropPrefabsLoaded += RegisterProduct;
        GameLoader.OnAllPrefabsLoaded += RegisterAllPrefabs;
        LevelManager.OnLevelLoaded += RegisterLevel;
        LevelBuilder.OnPlayerStartPositionReady += HandlePlayerStartPositionReady;
    }
    private void Initialize()
    {
        Loader.LoadAllPrefabs();
        OnBootGame?.Invoke(0f);
    }
    void OnDestroy()
    {
        GameLoader.OnPropPrefabsLoaded -= RegisterProduct;
        GameLoader.OnAllPrefabsLoaded -= RegisterAllPrefabs;
        LevelManager.OnLevelLoaded -= RegisterLevel;
        LevelBuilder.OnPlayerStartPositionReady -= HandlePlayerStartPositionReady;
    }
    private void RegisterProduct(string id, Prop product)
    {
        Factory.RegisterProduct(id, product.gameObject);
    }

    private void RegisterAllPrefabs()
    {
        OnBootGame?.Invoke(0.2f);
        LevelManager.LoadLevelAsync(1);
    }
    private void RegisterLevel(LevelData level)
    {
        OnBootGame?.Invoke(0.5f);
        StartCoroutine(levelBuilder.BuildMapAsync(level, progress =>
        {
            OnBootGame?.Invoke(progress);
        }));
    }
    private void HandlePlayerStartPositionReady(Vector3 startPosition)
    {
        OnBootGame?.Invoke(1f);
        playerController.gameObject.SetActive(true);
        playerController.transform.position = startPosition;
    }


    public void HandleGameWin()
    {
        
    }

    public void EnableInput()
    {
        eventSystem.enabled = true;
        OnInputStateChanged?.Invoke(true);

    }
    public void DisableInput()
    {
        eventSystem.enabled = false;
        OnInputStateChanged?.Invoke(false);
    }

}
