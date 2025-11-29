using BaseEngine;
using DG.Tweening;
using UnityEngine;

public class DashboardScreen : BaseScreen
{
    [SerializeField] private MagicButton startButton;

    private void Start()
    {
        startButton.AddListener(OnStartButtonClicked);
        PlayStartButtonAnim();
    }
    void OnDestroy()
    {
        startButton.RemoveListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {

        GameController.Instance.DisableInput();
        StartCoroutine(UIManager.Instance.CloseScreenAsync<DashboardScreen>());
        StartCoroutine(UIManager.Instance.OpenScreenAsync<GameScreen>());
        GameController.Instance.EnableInput();
    }

    private void PlayStartButtonAnim()
    {
        var img = startButton.GetComponent<UnityEngine.UI.Image>();
        img.DOFade(0.2f, 0.5f)
           .SetLoops(-1, LoopType.Yoyo);

    }
}
