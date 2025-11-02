using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static event Action<bool> OnInputStateChanged;
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
