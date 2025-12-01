using DG.Tweening;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public TMP_Text text;
    public float minAlpha = 0.3f;
    public float duration = 0.5f;

    private void Start()
    {
        if (text == null) text = GetComponent<TMP_Text>();

        // Create a looping tween
        text.DOFade(minAlpha, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.Linear);
    }
}
