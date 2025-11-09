using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StarBackground : MonoBehaviour
{
    [SerializeField] private int starCount = 100;
    [SerializeField] private Sprite[] starSprites;
    [SerializeField] private float minSpeed = 0f;
    [SerializeField] private float maxSpeed = 5f;

    private RectTransform rect;
    private readonly List<Star> stars = new();

    private class Star
    {
        public RectTransform transform;
        public float speed;
    }

    void OnEnable()
    {
        rect = GetComponent<RectTransform>();

        for (int i = 0; i < starCount; i++)
        {
            CreateStar();
        }
    }

    void OnDisable()
    {
        foreach (var star in stars)
        {
            if (star.transform != null)
            {
                Destroy(star.transform.gameObject);
            }
        }
        stars.Clear();
    }

    void Update()
    {
        foreach (var star in stars)
        {
            var pos = star.transform.anchoredPosition;
            pos.y -= star.speed * Time.deltaTime;
            pos.x += star.speed * Time.deltaTime * 0.5f;

            // When off-screen, reset to top
            if (pos.y < -rect.rect.height / 2f)
                pos.y = rect.rect.height / 2f;

            if (pos.x > rect.rect.width / 2f)
                pos.x = -rect.rect.width / 2f;

            star.transform.anchoredPosition = pos;
        }
    }

    void CreateStar()
    {
        GameObject starObj = new("Star");
        starObj.transform.SetParent(transform, false);

        Image img = starObj.AddComponent<Image>();
        img.sprite = starSprites[Random.Range(0, starSprites.Length)];
        img.color = new Color(1f, 1f, 1f, Random.Range(0.7f, 1f));

        var starSize = Random.Range(1f, 3f);

        RectTransform rt = img.rectTransform;
        rt.sizeDelta = Vector2.one * starSize;

        rt.anchoredPosition = new Vector2(
            Random.Range(-rect.rect.width / 2f, rect.rect.width / 2f),
            Random.Range(-rect.rect.height / 2f, rect.rect.height / 2f)
        );

        Star star = new() { transform = rt, speed = Random.Range(minSpeed, maxSpeed) + starSize};
        stars.Add(star);
    }
}
