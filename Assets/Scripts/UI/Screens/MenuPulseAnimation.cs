using UnityEngine;
using DG.Tweening; // Make sure DOTween is installed and imported!

public class MenuPulseAnimation : MonoBehaviour
{
    [Header("Settings")]
    public RectTransform target;       // The UI element to scale
    public float scaleUp = 1.2f;       // Maximum scale (e.g. 1.2 = 120%)
    public float duration = 0.8f;      // Time to scale up/down
    public Ease easeType = Ease.InOutSine; // Animation curve

    private Tween pulseTween;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        StartPulse();
    }

    void StartPulse()
    {
        // Create the looping pulse tween
        pulseTween = target.DOScale(scaleUp, duration)
            .SetEase(easeType)
            .SetLoops(-1, LoopType.Yoyo); // Infinite loop, back and forth
    }
}
