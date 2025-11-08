using System.Collections;
using BaseEngine;
using UnityEngine;

public class DashboardScreen : BaseScreen
{
    [SerializeField] private MagicButton startButton;
    [SerializeField] private MagicButton exitButton;

    private void Start()
    {
        startButton.AddListener(OnStartButtonClicked);
        exitButton.AddListener(OnExitButtonClicked);
    }
    void OnDestroy()
    {
        startButton.RemoveListener(OnStartButtonClicked);
        exitButton.RemoveListener(OnExitButtonClicked);
    }

    private void OnStartButtonClicked()
    {

        GameController.Instance.DisableInput();
        StartCoroutine(UIManager.Instance.CloseScreenAsync<DashboardScreen>());
        StartCoroutine(UIManager.Instance.OpenScreenAsync<GameScreen>());
        GameController.Instance.EnableInput();
    }
    private void OnExitButtonClicked()
    {
        Application.Quit();
    }
}
