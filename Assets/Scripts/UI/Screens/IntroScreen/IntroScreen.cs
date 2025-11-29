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
        // yield return base.OpenAsync();
        yield return null;
        PlayIntroAnim();
    }
    public void PlayIntroAnim()
    {
        seq?.Kill();
        seq = DOTween.Sequence();

        for (int i = 0; i < introSprites.Count; i++)
        {
            int spriteIndex = i;

            int currImgIndex = i % introImages.Count;
            int nextImgIndex = (i + 1) % introImages.Count;

            RectTransform currRT = introImages[currImgIndex].rectTransform;
            RectTransform nextRT = introImages[nextImgIndex].rectTransform;

            // Prepare next image
            seq.AppendCallback(() =>
            {
                introImages[currImgIndex].sprite = introSprites[spriteIndex];
                introImages[nextImgIndex].sprite = introSprites[
                    (spriteIndex + 1) % introSprites.Count
                ];

                currRT.anchoredPosition = new Vector2(showPositionX, currRT.anchoredPosition.y);
                nextRT.anchoredPosition = new Vector2(rightHidePositionX, nextRT.anchoredPosition.y);
            });

            // Animate both at the same time
            seq.Append(currRT.DOAnchorPosX(leftHidePositionX, 1f));
            seq.Join(nextRT.DOAnchorPosX(showPositionX, 1f));

            seq.AppendInterval(0.5f);
        }

        seq.OnComplete(() =>
        {
            StartCoroutine(UIManager.Instance.CloseScreenAsync<IntroScreen>());
            StartCoroutine(UIManager.Instance.OpenScreenAsync<GameScreen>());
        });

        seq.Play();
    }
}
