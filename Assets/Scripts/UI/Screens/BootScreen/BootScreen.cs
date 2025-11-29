using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
public class BootScreen : BaseScreen
{
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private TMP_Text progressText;

    private Coroutine loadingTextCoroutine;
    void Awake()
    {
        GameController.OnBootGame += HandleBootGameProgress;
    }
    void OnDestroy()
    {
        GameController.OnBootGame -= HandleBootGameProgress;
    }
    void Start()
    {
        PlayProgressTextAnim();
    }
    private void HandleBootGameProgress(float ratio)
    {
        progressBar.SetProgress(ratio);
        if (ratio == 1)
        {
            HandleBootGameFinished();
        }
    }
    private void HandleBootGameFinished()
    {
        StartCoroutine(UIManager.Instance.CloseScreenAsync<BootScreen>());
        StartCoroutine(UIManager.Instance.OpenScreenAsync<DashboardScreen>());
    }
    private void PlayProgressTextAnim()
    {
        progressText.transform.DOKill();
        progressText.alpha = 0f;
        Tween fadeTween = progressText.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        fadeTween.Play();
        if (loadingTextCoroutine != null)
            StopCoroutine(loadingTextCoroutine);
        loadingTextCoroutine = StartCoroutine(AnimateLoadingText());
    }

    private IEnumerator AnimateLoadingText()
    {
        string baseText = "loading";
        string[] dots = { ".", "..", "...", "" };
        int i = 0;
        while (true)
        {
            progressText.text = baseText + dots[i];
            i = (i + 1) % dots.Length;
            yield return new WaitForSeconds(0.5f);
        }
    }

}
