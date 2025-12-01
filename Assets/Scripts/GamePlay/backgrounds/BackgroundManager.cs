using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform bg1;
    [SerializeField] private Transform bg2;

    [SerializeField] private float baseX = 0;
    [SerializeField] private float baseY = -90;
    [SerializeField] private float distance = 375;

    private Transform leftBG;
    private Transform rightBG;

    void Start()
    {
        bg1.position = new Vector3(baseX, baseY, bg1.position.z);
        bg2.position = new Vector3(baseX + distance, baseY, bg2.position.z);

        leftBG = bg1;
        rightBG = bg2;
    }

    void LateUpdate()
    {
        float playerX = player.position.x;

        if (playerX > rightBG.position.x)
        {
            leftBG.position = new Vector3(
                rightBG.position.x + distance,
                baseY,
                leftBG.position.z
            );

            Swap();
        }

        if (playerX < leftBG.position.x)
        {
            rightBG.position = new Vector3(
                leftBG.position.x - distance,
                baseY,
                rightBG.position.z
            );

            Swap();
        }
    }

    void Swap()
    {
        var temp = leftBG;
        leftBG = rightBG;
        rightBG = temp;
    }
}
