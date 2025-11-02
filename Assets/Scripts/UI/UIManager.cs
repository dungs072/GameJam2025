using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<BaseScreen> screens;
    private static UIManager _instance;
    public static UIManager Instance => _instance;
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public BaseScreen GetScreen<T>() where T : BaseScreen
    {
        foreach (var screen in screens)
        {
            if (screen is T)
            {
                return screen;
            }
        }
        return null;
    }
    public IEnumerator OpenScreenAsync<T>() where T : BaseScreen
    {
        BaseScreen screen = GetScreen<T>();
        if (screen != null)
        {
            yield return screen.OpenAsync();
        }
        else
        {
            Debug.LogWarning($"Screen of type {typeof(T)} not found.");
        }
    }
    public IEnumerator CloseScreenAsync<T>() where T : BaseScreen
    {
        BaseScreen screen = GetScreen<T>();
        if (screen != null)
        {
            yield return screen.CloseAsync();
        }
        else
        {
            Debug.LogWarning($"Screen of type {typeof(T)} not found.");
        }
    }
}
