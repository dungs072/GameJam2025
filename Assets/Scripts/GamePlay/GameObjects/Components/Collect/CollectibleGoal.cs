using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleGoal : MonoBehaviour, IPropComponent
{
    private ProductData productData;
    public SpriteRenderer targetImage;
    public float durationPerColor = 0.5f;
    void Awake()
    {
        productData = GetComponent<Prop>().productData;
    }

    void Start()
    {
        StartColorCycle();
    }

    void StartColorCycle()
    {
        Color[] colors = new Color[]
        {
            Color.white,
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            new Color(0.5f, 0f, 1f), // purple
            Color.white
        };

        Sequence seq = DOTween.Sequence();

        for (int i = 0; i < colors.Length; i++)
        {
            seq.Append(targetImage.DOColor(colors[i], durationPerColor));
        }

        seq.SetLoops(-1, LoopType.Restart);
    }
    
    public bool HandleInteractWithCharacter(ICharacter character)
    {
        GameController.Instance.HandleGameWin();
        return true;
    }
}
