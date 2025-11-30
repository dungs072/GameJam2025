using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class IntroScreen : BaseScreen
{
    [SerializeField] private List<Sprite> introSprites;

    [SerializeField] private List<Image> introImages;
    private int leftHidePositionX = -2000;
    private int rightHidePositionX = 2000;
    private int showPositionX = 0;
    private Sequence seq;

    public override IEnumerator OpenAsync()
    {
        gameObject.SetActive(true);
        CanvasGroup.alpha = 0f;
        Tween fadeTween = CanvasGroup.DOFade(1f, 0.5f);
        fadeTween.Play();
        yield return null;
        PlayIntroAnim();
    }
    public void PlayIntroAnim()
    {
        seq?.Kill();
        seq = DOTween.Sequence();

        for (int i = 0; i < introSprites.Count - 1; i++)
        {
            int spriteIndex = i;

            int currImgIndex = i % introImages.Count;
            int nextImgIndex = (i + 1) % introImages.Count;

            var currImg = introImages[currImgIndex];
            var nextImg = introImages[nextImgIndex];

            RectTransform currRT = currImg.rectTransform;
            RectTransform nextRT = nextImg.rectTransform;

            CanvasGroup currCG = currImg.GetComponent<CanvasGroup>();
            CanvasGroup nextCG = nextImg.GetComponent<CanvasGroup>();

            seq.AppendCallback(() =>
            {
                currImg.sprite = introSprites[spriteIndex];
                nextImg.sprite = introSprites[(spriteIndex + 1) % introSprites.Count];

                // Reset states
                currRT.anchoredPosition = new Vector2(showPositionX, currRT.anchoredPosition.y);
                currRT.localScale = Vector3.one;
                currCG.alpha = 1f;

                nextRT.anchoredPosition = new Vector2(rightHidePositionX, nextRT.anchoredPosition.y);
                nextRT.localScale = Vector3.one * 0.85f;
                nextCG.alpha = 0f;
            });

            float stayDuration = i == 0 ? 5f : 2.5f;
            seq.AppendInterval(stayDuration);

            // OUT animation (current)
            seq.Append(currRT.DOAnchorPosX(leftHidePositionX, 0.8f)
                .SetEase(Ease.InCubic));
            seq.Join(currRT.DOScale(0.85f, 0.8f));
            seq.Join(currCG.DOFade(0f, 0.6f));

            // IN animation (next)
            seq.Join(nextRT.DOAnchorPosX(showPositionX, 0.9f)
                .SetEase(Ease.OutBack));
            seq.Join(nextRT.DOScale(1f, 0.9f));
            seq.Join(nextCG.DOFade(1f, 0.6f));
        }

        seq.AppendInterval(2f);

        seq.OnComplete(() =>
        {
            StartCoroutine(UIManager.Instance.CloseScreenAsync<IntroScreen>());
            StartCoroutine(UIManager.Instance.OpenScreenAsync<GameScreen>());
        });

        seq.Play();
    }
}
