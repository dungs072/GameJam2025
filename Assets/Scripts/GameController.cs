using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [field: SerializeField] public BaseFactory Factory { get; private set; }
    [field: SerializeField] public GameLoader Loader { get; private set; }
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
    }

    public void EnableInput()
    {
        UnityEngine.EventSystems.EventSystem.current.enabled = true;
        OnInputStateChanged?.Invoke(true);

    }
    public void DisableInput()
    {
        UnityEngine.EventSystems.EventSystem.current.enabled = false;
        OnInputStateChanged?.Invoke(false);
    }
}
