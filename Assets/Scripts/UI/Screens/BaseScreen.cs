using System.Collections;
using DG.Tweening;
using UnityEngine;

public class BaseScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private float fadeDuration = 0.5f;

    public IEnumerator OpenAsync()
    {
        gameObject.SetActive(true);
        yield return FadeInCoroutine();
    }

    protected IEnumerator FadeInCoroutine()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Tween fadeTween = canvasGroup.DOFade(1f, fadeDuration);

        yield return fadeTween.WaitForCompletion();
    }



    public IEnumerator CloseAsync()
    {
        yield return FadeOutCoroutine();
        gameObject.SetActive(false);
    }

    protected IEnumerator FadeOutCoroutine()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        Tween fadeTween = canvasGroup.DOFade(0f, fadeDuration);

        yield return fadeTween.WaitForCompletion();
    }
}
