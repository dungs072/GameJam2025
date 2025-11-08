using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [field: SerializeField] public BaseFactory Factory { get; private set; }
    [field: SerializeField] public GameLoader Loader { get; private set; }

    [Header("Data")]
    [field: SerializeField] public ColorRuler ColorRuler { get; private set; }
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
    }
    private void Initialize()
    {
        Loader.LoadAllPrefabs();
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
